const favouritesContainer = document.getElementById("userFavourites");
const take = 8;
let skip = 0;
let sortBy = "title-asc";

const moreButton = document.getElementById("moreButton");
const sortOptions = document.getElementById("sort");
const favouritesCount = document.getElementById("favouritesCount");

const handleChange = (e) => {
    sortBy = e.target.value;
    skip = 0;
    favouritesContainer.innerHTML="";
    moreButton.disabled = false;
    loadFavourites();
}

const getBook = (book) => {
    const markup = `
    <div class="user__favourites-item">
        <a href="/Book/Index/${book.Id}" class="user__favourites-figure">
            <img class="user__favourites-image" src="${book.Cover}" />
        </a>
        <button data-delete data-bookid="${book.Id}" class="user__favourites-button">
            Usuń z ulubionych
            <svg class="user__favourites-icon icon" viewBox="0 0 32 32">
                <use xlink:href="/assets/svg/sprite.svg#icon-heart-broken"></use>
            </svg>
        </button>
        <p class="user__favourites-title" title="${book.Title}">${book.Title}</p>
        <p class="user__favourites-author">${book.Authors}</p>
    </a>
    `;

    return markup;
}

const loadFavourites = () => {
    displaySpinner();
    $.ajax({
        type: "GET",
        url: "/User/GetAllUserFavourites",
        data: {skip: skip, count: take, sortBy: sortBy},
        dataType: 'json',
        success: function (favourites) {
            skip+=take;
            favourites.forEach(f => {
                let content = getBook(f);
                favouritesContainer.innerHTML+=(content);
            });
            hideSpinner();
            if(favourites.length ==0) {
                moreButton.removeEventListener('click',loadFavourites);
                moreButton.disabled = true;
            }
        },
        error: function (emp) {
            alert('Coś poszło nie tak :(');
        }
    });
}

const updateFavouritesCount = () => {
    let amount = parseInt(favouritesCount.innerHTML) -1;
    favouritesCount.innerHTML = amount;
}

const deleteReview = (id, element) => {
    displaySpinner();
    $.ajax({
        type: "DELETE",
        url: "/User/DeleteFavouriteBook",
        dataType: 'json',
        data: {bookId: id},
        success: function (result) {
            hideSpinner();
            element.remove();
            updateFavouritesCount();
        },
        error: function (emp) {
            alert("Coś poszło nie tak");
        }
    });
}

favouritesContainer.addEventListener('click',e=>{
    if(e.target.hasAttribute("data-delete")){
        const id = parseInt(e.target.dataset.bookid);
        const el = e.target.closest('.user__favourites-item');
        if(id) deleteReview(id,el);
    }
});

moreButton.addEventListener('click',loadFavourites);
sortOptions.addEventListener('change',handleChange);
loadFavourites();