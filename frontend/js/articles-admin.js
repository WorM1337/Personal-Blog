const body = document.querySelector('body')
const overlay = document.querySelector(".overlay")


const AddButton = document.querySelector(".add-btn")

function isInactive(tag) {
    return (!tag.hasAttribute("style")) || tag.hasAttribute("style") && tag.getAttribute('style') == "";
}

function toggleOverlay() {
    if(isInactive(overlay)) {
        overlay.setAttribute("style", "display: grid")
        body.setAttribute('style', "overflow: hidden;")
    }
    else {
        overlay.setAttribute("style", "")
        body.setAttribute('style', "")
        
    }
}
function toggleModal(modal) {
    if(isInactive(modal)) {
        modal.setAttribute("style", "display: flex")
    }
    else {
        modal.setAttribute("style", "")
    }
}

const addArticleCon = overlay.querySelector(".add-article-con")
const updateArticleCon = overlay.querySelector(".update-article-con")

AddButton.addEventListener('click', () => {
    toggleOverlay();
    toggleModal(addArticleCon)
})

overlay.addEventListener("click", (e) => {
    
    if(e.target == overlay || e.target.closest(".close-btn")) {
        toggleOverlay();
        toggleModal(addArticleCon)
    }
})



// Все эти события нужно будет накидывать на каждый article при подгрузке
const article = document.querySelector(".article");
console.log(article)
article.addEventListener('mouseenter', () => {
    const actions = article.querySelectorAll(".action-text");
    const date = article.querySelector(".date")
    if(Array.from(actions).length != 0) {
        if(isInactive(date)) {
            date.setAttribute('style', 'display: inline;')
            Array.from(actions).forEach((item) => {
                item.setAttribute('style', '')
            });
        }
        else {
            date.setAttribute('style', '')
            Array.from(actions).forEach((item) => {
                item.setAttribute('style', 'display: block')
            });
        }
    }
})
article.addEventListener('mouseleave', () => {
    const actions = article.querySelectorAll(".action-text");
    const date = article.querySelector(".date")
    if(Array.from(actions).length != 0) {
        if(isInactive(date)) {
            date.setAttribute('style', 'display: inline;')
            Array.from(actions).forEach((item) => {
                item.setAttribute('style', '')
            });
        }
        else {
            date.setAttribute('style', '')
            Array.from(actions).forEach((item) => {
                item.setAttribute('style', 'display: block')
            });
        }
    }
})
// TODO: Тут нужно будет сделать так, чтобы при нажатии на каждую из кнопок edit и delete вылетало окно с update и delete подтверждение
// Конец всех событий