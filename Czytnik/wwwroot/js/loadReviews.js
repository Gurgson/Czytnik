const moreButton = document.getElementById("more");
const reviewsContainer = document.getElementById("reviewsContainer");

const reviewsToTake=10;
const bookId = new URLSearchParams(location.search).get('id');
let skip = 0;

function generateAllReviews(reviews){
    reviews.forEach(review => {
        const reviewCont = document.createElement("div");
        reviewCont.className = `review review--${review.rating}`;

        const figureCont = document.createElement("figure");
        figureCont.className ="review__value";
        figureCont.textContent=review.rating;

        const reviewInfoCont = document.createElement("div");
        reviewInfoCont.className="review__info";
        const innerString = `oceniona przez: ${review.username}, ${review.reviewDate.slice(0,10)}`;
        reviewInfoCont.textContent= innerString;

        const reviewContent = document.createElement("p");
        reviewContent.className="review__content";
        reviewContent.textContent = review.reviewText;
        
        reviewCont.appendChild(figureCont);
        reviewCont.appendChild(reviewInfoCont);
        reviewCont.appendChild(reviewContent);
        reviewsContainer.appendChild(reviewCont);
    });
}

function getReviewsData() {
    $.ajax({
        type: 'GET',
        url: '/Reviews/GetAllReviews',
        data: {Id: bookId, skip: skip, take: reviewsToTake},
        dataType: 'json',
        success: function (reviews) {
            generateAllReviews(reviews);
            skip+=reviewsToTake;
            if(reviews.length ==0) {
                moreButton.removeEventListener('click',getReviewsData);
                moreButton.disabled = true;
            }
        },
        error: function (emp) {
            alert('error');
        }
    });
}

getReviewsData();

moreButton.addEventListener('click',getReviewsData);