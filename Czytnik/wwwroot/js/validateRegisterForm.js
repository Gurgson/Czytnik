const form = document.getElementById("form");
const emailInput = document.getElementById("emailInput");
const usernameInput = document.getElementById("usernameInput");
const passwordInput = document.getElementById("passwordInput");
const password2Input = document.getElementById("password2Input");

const formTruthTable = {
    email: false,
    username: false,
    password: false,
    password2: false
};

emailInput.addEventListener('change',e=>{
    formTruthTable.email = validateEmail(e.target);
    if(!formTruthTable.email){
        e.target.parentNode.classList.add('form__label--error');
    } else {
        e.target.parentNode.classList.remove('form__label--error');
    }
});

passwordInput.addEventListener('change',e=>{
    formTruthTable.password = validatePassword(e.target);
    formTruthTable.password2= validateSecondPassword(password2Input,passwordInput.value);
    if(!formTruthTable.password){
        e.target.parentNode.classList.add('form__label--error');
    } else {
        e.target.parentNode.classList.remove('form__label--error');
    }
});

password2Input.addEventListener('change',e=>{
    formTruthTable.password2 = validateSecondPassword(e.target,passwordInput.value);
    if(!formTruthTable.password2){
        e.target.parentNode.classList.add('form__label--error');
    } else {
        e.target.parentNode.classList.remove('form__label--error');
    }
});

usernameInput.addEventListener('change', e=>{
    formTruthTable.username= validateUsername(e.target);
    if(!formTruthTable.username){
        e.target.parentNode.classList.add('form__label--error');
    } else {
        e.target.parentNode.classList.remove('form__label--error');
    }
});

form.addEventListener('submit',e=>{
    e.preventDefault();
    console.log(formTruthTable);
    let allow = true;
    for (const [key, value] of Object.entries(formTruthTable)) {
        allow *= value;
    }
    if(allow)form.submit();
});