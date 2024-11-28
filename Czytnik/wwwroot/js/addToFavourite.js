const favButton = document.querySelector('.js-product-favourite');
const productId = parseInt(favButton.dataset.bookid);

const handleButtonClick = () => {
    switch(favButton.dataset.type){
        case "add":
            addToFavourites();
            break;
        case "delete":
            deleteFromFavourites();
            break;
    }
}

const createDeleteButton = () => {
    favButton.dataset.type="delete";
    favButton.classList.add("product__favourite--delete");
    favButton.innerHTML = `
        <svg class="product__favourite-icon icon" viewBox="0 0 32 32">
            <use xlink:href="/assets/svg/sprite.svg#icon-heart-broken"></use>
        </svg>
        Usuń z ulubionych
    `;
}

const createAddButton = () => {
    favButton.dataset.type="add";
    favButton.classList.remove("product__favourite--delete");
    favButton.innerHTML = `
        <svg class="product__favourite-icon icon" viewBox="0 0 32 32">
            <use xlink:href="/assets/svg/sprite.svg#icon-heart"></use>
        </svg>
        Dodaj do ulubionych
    `;
}

const showWait = (show) => {
    show ? favButton.classList.add('wait'): favButton.classList.remove('wait');
}

const addToFavourites = () => {
    showWait(1);
    $.ajax({
        type: "POST",
        url: "/User/AddFavouriteBook",
        dataType: 'json',
        data: {bookId: productId},
        success: function (result) {
            showWait(0);
            createDeleteButton();
        },
        error: function (emp) {
            alert("Coś poszło nie tak");
        }
    });
}

const deleteFromFavourites = () => {
    showWait(1);
    $.ajax({
        type: "DELETE",
        url: "/User/DeleteFavouriteBook",
        dataType: 'json',
        data: {bookId: productId},
        success: function (result) {
            showWait(0);
            createAddButton();
        },
        error: function (emp) {
            alert("Coś poszło nie tak");
        }
    });
}

favButton.addEventListener('click', handleButtonClick);