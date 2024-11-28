const form = document.getElementById("reviewForm");
const reviewInput = form.querySelector('[name="reviewText"');
const ratingInput = form.querySelector('[name="rating"');

const charCount = document.getElementById("charCount");
let url = window.location.pathname.split('/');
const bookId = url[url.length-1];

const getFormData = () => {
    let review = reviewInput.value;
    let rating = parseInt(ratingInput.value);

    return [review.trim(), rating];
}

const updatePage = () => {
    form.innerText="Twoja recenzja została dodana";
    document.querySelectorAll('.js-count').forEach(n=>{
        var count = parseInt(n.innerText)+1;
        n.innerText = count;
    })
}

const validateReviewForm = ([text,rating]) => {

    let isRating = (rating > 0) && (rating <= 6);
    let isText = text.length < 1000;

    return isRating * isText;
}

const calculateCharacters = () => {
    let wordCount = reviewInput.value.length;
    charCount.innerText=`${wordCount} / 999`;
    if(wordCount>999) charCount.classList.add('review-form__charCount--warning');
    else charCount.classList.remove('review-form__charCount--warning');
}

const sendFormData = () => {
    const [reviewText, rating] = getFormData();
    displaySpinner();
    $.ajax({
        type: 'POST',
        url: '/Reviews/Add',
        data: {ReviewText: reviewText, Rating: rating, BookId: bookId},
        success: function (result) {
            hideSpinner();
            updatePage();
        },
        error: function (emp) {
            form.innerText="Przepraszamy, coś poszło nie tak.";
        }
    });
}

const disableButton = () => {
    const button = document.getElementById("submitReview");
    button.disabled = true;
}

form.addEventListener('submit', async e => {
    e.preventDefault();
    if(validateReviewForm(getFormData())){
        sendFormData();
    }
});

reviewInput.addEventListener('input', calculateCharacters);
