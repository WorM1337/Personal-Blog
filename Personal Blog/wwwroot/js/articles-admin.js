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

AddButton.addEventListener("click", () => {
  toggleOverlay();
  toggleModal(addArticleCon);
});

overlay.addEventListener("click", (e) => {
  if (e.target == overlay || e.target.closest(".close-btn")) {
    toggleOverlay();
    toggleModal(addArticleCon);
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
function ViewAllArticles() {
  articlesCon.innerHTML = "";
  articlesData.forEach((item) => {
    const article = document.createElement("div");
    article.classList.add("article");
    article.innerHTML = `<span class="article-title">${item.title}</span>
          <div class="side-article-con">
            <span class="date" style="display: inline;">${item.date}</span>
            <button class="action-text">Edit</button>
            <button class="action-text">Delete</button>
          </div>`;

    console.log(article);
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
    article.addEventListener("click", (e) => {
      if (e.target == article) {
        toggleOverlay();
        toggleModal(readArticleCon);
      }
    });
    articlesCon.appendChild(article);
  });
}
firstData()
// Все эти события нужно будет накидывать на каждый article при подгрузке
// TODO: Нужно, чтобы подгружались название, дата и текст статьи
// TODO: Тут нужно будет сделать так, чтобы при нажатии на каждую из кнопок edit и delete вылетало окно с update и delete подтверждение
