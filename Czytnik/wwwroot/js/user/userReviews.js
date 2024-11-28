const reviewsContainer = document.getElementById("userReviews");
const take = 5;
let skip = 0;
let sortBy = "date-desc";

const moreButton = document.getElementById("moreButton");
const sortOptions = document.getElementById("sort");
const reviewsCount = document.getElementById("reviewsCount");

const handleChange = (e) => {
    sortBy = e.target.value;
    skip = 0;
    reviewsContainer.innerHTML="";
    moreButton.disabled = false;
    loadReviews();
}

const createButton = (value,type, variant="",id) => {
    const button = document.createElement('button');
    button.classList.add("button");
    button.classList.add(`button--${variant}`);
    button.innerText = value;
    button.dataset.type = type;
    button.dataset.id = id;

    return button;
}

const getNewReviewData = (id) => {
    const box =  document.querySelector(`[data-review='${id}']`);
    let reviewText = box.querySelector('.userReview__textarea').value;
    const reviewRating = parseInt(box.querySelector('.rating').value);

    reviewText = reviewText ? reviewText : "";

    return {ReviewText: reviewText, Rating: reviewRating};
}

const updateReviewBox = (id,data) => {
    const box =  document.querySelector(`[data-review='${id}']`);
    const reviewText = box.querySelector('.userReview__text');
    const reviewRating = box.querySelector('.userReview__rating');

    reviewText.innerText = data.ReviewText;
    reviewRating.innerText = data.Rating;
}

const submitReview = (id) => {
    const data = getNewReviewData(id);
    displaySpinner();
    $.ajax({
        type: "PATCH",
        url: "/Reviews/Edit",
        dataType: 'json',
        data: {Review:{...data},Id: id},
        success: function (result) {
            hideSpinner();
            cancelReview(id);
            updateReviewBox(id,data);
        },
        error: function (emp) {
            alert("Coś poszło nie tak");
            hideSpinner();
        }
    });
}

const cancelReview = (id) => {
    const reviewBox = document.querySelector(`[data-review='${id}']`);

    reviewBox.querySelector("[data-type='edit']").style.display = 'block';
    reviewBox.querySelector("[data-type='delete']").style.display = 'block';
    reviewBox.querySelector('.userReview__text').style.display = 'block';

    reviewBox.querySelector('.userReview__textarea').remove();
    reviewBox.querySelector('.rating-label').remove();
    reviewBox.querySelector("[data-type='save']").remove();
    reviewBox.querySelector("[data-type='cancel']").remove();
    reviewBox.classList.remove('userReview--editable');
}

const displayEditForm = (id) => {
    const reviewBox = document.querySelector(`[data-review='${id}']`);
    reviewBox.classList.add('userReview--editable');

    const reviewText = reviewBox.querySelector('.userReview__text');

    const textArea = document.createElement('textarea');
    textArea.value = reviewText.innerText;
    textArea.classList.add('userReview__textarea');
    textArea.setAttribute('max',999);

    reviewText.style.display = 'none';
    reviewBox.querySelector('.userReview__column').append(textArea);
    textArea.focus();

    const rating = reviewBox.querySelector('.userReview__rating').innerText;
    const stars = document.createElement('label');
    stars.classList.add('rating-label');

    const starsInput = document.createElement('input');
    starsInput.classList.add('rating');
    starsInput.setAttribute('max',6);
    starsInput.setAttribute('min',1);
    starsInput.setAttribute('step',1);
    starsInput.setAttribute('style',`--stars:6;--value:${rating}`);
    starsInput.setAttribute('type',"range");
    starsInput.setAttribute('value',rating);
    starsInput.setAttribute('name',rating);
    starsInput.setAttribute('required',true);
    starsInput.addEventListener('input',e=>e.target.style.setProperty('--value', `${e.target.value}`))

    stars.appendChild(starsInput);
    reviewBox.querySelector('.userReview__column').prepend(stars);

    reviewBox.querySelector("[data-type='edit']").style.display = 'none';
    reviewBox.querySelector("[data-type='delete']").style.display = 'none';

    reviewBox.querySelector('.userReview__buttons').append(createButton("ZAPISZ","save","",id));
    reviewBox.querySelector('.userReview__buttons').append(createButton("ANULUJ","cancel","danger",id));
}

const getReview = (review) => {
    let text = review.ReviewText;
    text = text ? text.replace(/</g, "&lt;").replace(/>/g, "&gt;") : "";
    const markup = `
    <li class="userReview userReview--buttons" data-review="${review.Id}">
        <div class="userReview__column">
            <div class="userReview__header">
                <div>
                    <svg class="userReview__icon icon" viewBox="0 0 32 32">
                        <use xlink:href="/assets/svg/sprite.svg#icon-star"></use>
                    </svg>
                    <span class="userReview__rating">${review.Rating}</span>
                </div>
                <span class="userReview__date">${review.ReviewDate.slice(0, 10)}</span>
            </div>
            <a class="userReview__title" href="/Book/Index/${review.BookId}" title="${review.BookTitle}">${
      review.BookTitle
    } - <span class="userReview__author">${review.Authors}</span></a>
            <p class="userReview__text">
                ${text}
            </p>
        </div>
        <div class="userReview__buttons">
            <button class="button button--secondary" data-type="edit" data-id="${review.Id}">EDYTUJ</button>
            <button class="button button--danger" data-type="delete" data-id="${review.Id}">USUŃ</button>
        </div>
    </li>
    `;

    return markup;
}

const loadReviews = () => {
    displaySpinner();
    $.ajax({
        type: "GET",
        url: "/Reviews/GetAllUserReviews",
        data: {skip: skip, count: take, sortBy: sortBy},
        dataType: 'json',
        success: function (reviews) {
            skip+=take;
            reviews.forEach(r => {
                let content = getReview(r);
                reviewsContainer.innerHTML+=(content);
            });
            hideSpinner();
            if(reviews.length ==0) {
                moreButton.removeEventListener('click',loadReviews);
                moreButton.disabled = true;
            }
        },
        error: function (emp) {
            alert('Coś poszło nie tak :(');
        }
    });
}

const updateReviewsCount = () => {
    let amount = parseInt(reviewsCount.innerHTML) -1;
    reviewsCount.innerHTML = amount;
}

const deleteReview = (id, element) => {
    $.ajax({
        type: "DELETE",
        url: "/Reviews/Delete",
        data: {Id: id},
        dataType: 'json',
        success: function (result) {
            element.innerHTML="Pomyślnie usunięto recenzje";
            updateReviewsCount();
        },
        error: function (emp) {
            alert("Coś poszło nie tak");
        }
    });
}

moreButton.addEventListener('click',loadReviews);
sortOptions.addEventListener('change',handleChange);
loadReviews();

reviewsContainer.addEventListener('click',e=>{
    if(e.target.dataset.type === "delete"){
        let id = parseInt(e.target.dataset.id);
        e.target.disabled = true;
        deleteReview(id,e.target.closest('.userReview'));
    }
    if(e.target.dataset.type === "edit"){
        let id = parseInt(e.target.dataset.id);
        displayEditForm(id);
    }
    if(e.target.dataset.type === "save"){
        let id = parseInt(e.target.dataset.id);
        e.target.disabled = true;
        submitReview(id);
    }
    if(e.target.dataset.type === "cancel"){
        let id = parseInt(e.target.dataset.id);
        cancelReview(id);
    }
});
