const ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

const alphabetNavigation = document.getElementById("alphabetNavigation");
const categoriesListContainer = document.getElementById("categoriesList");

[...ALPHABET].forEach(letter => alphabetNavigation.innerHTML += (`<a href="#${letter}">${letter}</a>`));

const generateAlphabet = (data) => {
    let letters = data.map(category => category.category.name[0]);
    return letters.filter((element, index) => letters.indexOf(element) === index);
}

const createCategoryBoxElement = (letter, categoriesByLetter) => {
    let listItem = document.createElement("li");
    listItem.id = letter;
    listItem.className = "categories__list-item categoryBox";

    let itemHeader = document.createElement("a");
    itemHeader.className = "categoryBox__header";
    itemHeader.setAttribute("href",`#${letter}`);
    itemHeader.textContent = letter;

    let itemList = document.createElement("ul");
    itemList.className = "categoryBox__list";

    categoriesByLetter.map(category => {
        let itemCategory = document.createElement("li");
        itemCategory.className = "categoryBox__list-item";

        let itemAnchor = document.createElement("a");
        itemAnchor.setAttribute("href", `/Search?CategoryId=${category.category.id}`); //TODO: zrobi� dzia�aj�cy link
        itemAnchor.className = "categoryBox__list-link";
        itemAnchor.textContent = `${category.category.name} (${category.bookCount})`;

        itemCategory.appendChild(itemAnchor);
        itemList.appendChild(itemCategory);
    });

    listItem.appendChild(itemHeader);
    listItem.appendChild(itemList);
    return listItem;
}

const generateLayout = (data) => {
    let letters = generateAlphabet(data);
    letters.map(letter => {
        let categoriesByLetter = data.filter(category => category.category.name[0] === letter);
        let listItem = createCategoryBoxElement(letter, categoriesByLetter);
        
        categoriesListContainer.appendChild(listItem);
    });
}

function getCategoriesData() {
    displaySpinner();
    $.ajax({
        type: 'GET',
        url: 'Categories/GetAllCategories',
        dataType: 'json',
        success: function (categories) {
            hideSpinner();
            generateLayout(categories);
        },
        error: function (emp) {
            alert('error');
        }
    });
}

getCategoriesData();