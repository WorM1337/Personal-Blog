function IsAdminPage() {
  return window.location.href == "http://localhost:5000/html/admin.html";
}

function FormatDate(date) {
  return date.split("T")[0] || date;
}

if (localStorage.getItem("jwtToken") == null && IsAdminPage()) {
  window.location.href = "./articles.html";
}

const body = document.querySelector("body");
const overlay = document.querySelector(".overlay");

const AddButton = document.querySelector(".add-btn");

function isInactive(tag) {
  return (
    !tag.hasAttribute("style") ||
    (tag.hasAttribute("style") && tag.getAttribute("style") == "")
  );
}

function toggleOverlay() {
  if (isInactive(overlay)) {
    overlay.setAttribute("style", "display: grid");
    body.setAttribute("style", "overflow: hidden;");
  } else {
    overlay.setAttribute("style", "");
    body.setAttribute("style", "");
  }
}
function toggleModal(modal) {
  if (isInactive(modal)) {
    modal.setAttribute("style", "display: flex");
  } else {
    modal.setAttribute("style", "");
  }
}

const addArticleCon = overlay.querySelector(".add-article-con");
const updateArticleCon = overlay.querySelector(".update-article-con");
const readArticleCon = overlay.querySelector(".read-article-con");

AddButton?.addEventListener("click", () => {
  toggleOverlay();
  toggleModal(addArticleCon);
});

overlay.addEventListener("click", (e) => {
  if (e.target == overlay || e.target.closest(".close-btn")) {
    toggleOverlay();

    // Здесь нужен toggle для текущей активной модалки
    if (!isInactive(addArticleCon)) toggleModal(addArticleCon);
    else if (!isInactive(updateArticleCon)) toggleModal(updateArticleCon);
    else if (!isInactive(readArticleCon)) toggleModal(readArticleCon);
  }
});

class Article {
  constructor(id, title, date, text) {
    this.id = id;
    this.title = title;
    this.date = date;
    this.text = text;
  }
}
const articlesData = [];
const articlesCon = document.querySelector(".articles-con");
async function firstData() {
  const response = await fetch("http://localhost:5000/api/articles/getAll", {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  });
  if (!response.ok) throw Error("Не получилось получить статьи");

  const data = await response.json();

  Array.from(data).forEach((item) => {
    let newArticleData = new Article(item.id, item.title, item.date, item.text);
    articlesData.push(newArticleData);
  });

  ViewAllArticles();
}
function updateReadCon(art) {
  const titleCon = readArticleCon.querySelector(".article-title");
  const dateCon = readArticleCon.querySelector(".article-date");
  const textCon = readArticleCon.querySelector(".article-text");
  if (art == undefined) {
    titleCon.innerHTML = "";
    dateCon.innerHTML = "";
    textCon.innerHTML = "";
  } else {
    titleCon.innerHTML = art.title;
    dateCon.innerHTML = FormatDate(art.date);
    textCon.innerHTML = art.text;
  }
}
function updateUpdateCon(art) {
  const titleCon = document.getElementById("update-title");
  const dateCon = document.getElementById("update-date");
  const textCon = document.getElementById("update-text");
  if (art == undefined) {
    titleCon.value = "";
    dateCon.value = "";
    textCon.value = "";
  } else {
    if (art.title != undefined) titleCon.value = art.title;
    if (art.date != undefined) dateCon.value = FormatDate(art.date);
    if (art.text != undefined) textCon.value = art.text;
  }
}
var currentUpdateArticleId = null;
function ViewAllArticles() {
  articlesCon.innerHTML = "";
  articlesData.forEach((item) => {
    const article = document.createElement("div");
    article.classList.add("article");
    article.innerHTML = `<span class="article-title">${item.title}</span>
          <div class="side-article-con">
            <span class="date" style="display: inline;">${FormatDate(item.date)}</span>
            <button class="action-text">Edit</button>
            <button class="action-text">Delete</button>
          </div>`;
    const buttons = article.querySelectorAll("button");
    const editBtn = buttons[0];
    const deleteBtn = buttons[1];

    editBtn.addEventListener("click", () => {
      toggleOverlay();
      updateUpdateCon(item);
      toggleModal(updateArticleCon);
      currentUpdateArticleId = item.id;
    });
    deleteBtn.addEventListener("click", async () => {
      let isConfirmed = confirm("Do you really want to delete?");
      if (isConfirmed) {
        const response = await fetch(
          `http://localhost:5000/api/articles/delete/${item.id}`,
          {
            method: "DELETE",
            headers: {
              "Content-Type": "application/json",
              Authorization: "Bearer " + localStorage.getItem("jwtToken"),
            },
          },
        );
        if (currentUpdateArticleId == item.id) currentUpdateArticleId = null;
        location.reload();
      }
    });

    if (IsAdminPage()) {
      article.addEventListener("mouseenter", () => {
        const actions = article.querySelectorAll(".action-text");
        const date = article.querySelector(".date");
        if (Array.from(actions).length == 2) {
          if (isInactive(date)) {
            date.setAttribute("style", "display: inline;");
            Array.from(actions).forEach((item) => {
              item.setAttribute("style", "");
            });
          } else {
            date.setAttribute("style", "");
            Array.from(actions).forEach((item) => {
              item.setAttribute("style", "display: block");
            });
          }
        }
      });
      article.addEventListener("mouseleave", () => {
        const actions = article.querySelectorAll(".action-text");
        const date = article.querySelector(".date");
        if (Array.from(actions).length == 2) {
          if (isInactive(date)) {
            date.setAttribute("style", "display: inline;");
            Array.from(actions).forEach((item) => {
              item.setAttribute("style", "");
            });
          } else {
            date.setAttribute("style", "");
            Array.from(actions).forEach((item) => {
              item.setAttribute("style", "display: block");
            });
          }
        }
      });
    }

    article.addEventListener("click", (e) => {
      if (e.target == article) {
        updateReadCon(item);
        toggleOverlay();
        toggleModal(readArticleCon);
      }
    });
    articlesCon.appendChild(article);
  });
}
firstData();

async function AddArticle() {
  const addForm = document.getElementById("add-form");
  const addTitle = document.getElementById("add-title");
  const addDate = document.getElementById("add-date");
  const addText = document.getElementById("add-text");
  //const addSubmit = addForm.getElementById(".add-sunmit")

  addForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    const response = await fetch("http://localhost:5000/api/articles/add", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + localStorage.getItem("jwtToken"),
      },
      body: JSON.stringify({
        title: addTitle.value,
        date: addDate.value,
        text: addText.value,
      }),
    });
    if (response.ok) {
      window.location.href = "./admin.html";
    }
  });
}
async function UpdateArticle() {
  const updateForm = document.getElementById("update-form");
  const updateTitle = document.getElementById("update-title");
  const updateDate = document.getElementById("update-date");
  const updateText = document.getElementById("update-text");
  //const addSubmit = addForm.getElementById(".add-sunmit")

  updateForm.addEventListener("submit", async (e) => {
    e.preventDefault();
    const response = await fetch(
      `http://localhost:5000/api/articles/update/${currentUpdateArticleId}`,
      {
        method: "PATCH",
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + localStorage.getItem("jwtToken"),
        },
        body: JSON.stringify({
          title: updateTitle.value,
          date: updateDate.value,
          text: updateText.value,
        }),
      },
    );
    if (response.ok) {
      window.location.href = "./admin.html";
    }
  });
}
UpdateArticle();
AddArticle();

const logoutBtn = document.getElementById("logout");
logoutBtn?.addEventListener("click", () => {
  localStorage.removeItem("jwtToken");
});
