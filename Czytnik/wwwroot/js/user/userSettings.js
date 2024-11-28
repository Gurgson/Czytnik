const profileSettingsForm = document.getElementById("profileSettingsForm");

const profilePictureInput = document.getElementById("settingsImageInput");
const profilePicture = document.getElementById("settingsImage");
const deletePictureButton = document.getElementById("deleteUserPicture");
const profileSettingsButton = document.getElementById("profileSettingsButton");

const passwordSettingsForm = document.getElementById("passwordSettingsForm");

const deleteAccountSettingsForm = document.getElementById("deleteAccountSettingsForm");


const profileAccountFormTruthTable = {
    username: true
}

const passwordSettingsFormTruthTable = {
    currentPassword: false,
    newPassword: false,
    repeatNewPassword: false
}

profilePictureInput.addEventListener('change',e => {
    const [file] = e.target.files;
    if(file && validatePictureInput(e.target.value)){
        profilePicture.src = URL.createObjectURL(file);
    } else {
        e.target.value = "";
    }
});

profileSettingsForm.addEventListener("input",e=>{
    if(profileSettingsButton.disabled){
        profileSettingsButton.disabled=false;
    }
})

profileSettingsForm.addEventListener("change",e=>{
    var inputName = e.target.name;
    switch(inputName){
        case "firstName":
        case "secondName":
        case "surname":
            e.target.value = e.target.value.trim();
            break;
        case "phoneNumber":
            profileAccountFormTruthTable[inputName] = validatePhoneNumber(e.target);
            if(!profileAccountFormTruthTable[inputName]){
                e.target.parentNode.classList.add('small-input--error');
            } else {
                e.target.parentNode.classList.remove('small-input--error');
            }
            break;
        case "username":
            profileAccountFormTruthTable[inputName] = validateUsername(e.target);
            if(!profileAccountFormTruthTable[inputName]){
                e.target.parentNode.classList.add('small-input--error');
            } else {
                e.target.parentNode.classList.remove('small-input--error');
            }
            break
    }
});

passwordSettingsForm.addEventListener("change",e=>{
    var inputName = e.target.name;
    switch(inputName){
        case "currentPassword":
            passwordSettingsFormTruthTable[inputName] = validateString(e.target.value);
            if(!passwordSettingsFormTruthTable[inputName]){
                e.target.parentNode.classList.add('small-input--error');
            } else {
                e.target.parentNode.classList.remove('small-input--error');
            }
            break;
        case "newPassword":
            passwordSettingsFormTruthTable[inputName] = validatePassword(e.target);
            if(!passwordSettingsFormTruthTable[inputName]){
                e.target.parentNode.classList.add('small-input--error');
            } else {
                e.target.parentNode.classList.remove('small-input--error');
            }
            break;
        case "repeatNewPassword":
            let currpass = document.querySelector("input[name=newPassword]").value;
            console.log(currpass);
            passwordSettingsFormTruthTable[inputName] = validateSecondPassword(e.target,currpass);
            if(!passwordSettingsFormTruthTable[inputName]){
                e.target.parentNode.classList.add('small-input--error');
            } else {
                e.target.parentNode.classList.remove('small-input--error');
            }
            break;
    }
});

profileSettingsForm.addEventListener('submit',e=>{
    e.preventDefault();
    let allow = true;
    for (const [key, value] of Object.entries(profileAccountFormTruthTable)) {
        allow *= value;
    }
    if(allow)profileSettingsForm.submit();
});

passwordSettingsForm.addEventListener('submit',e=>{
    e.preventDefault();
    let allow = true;
    for (const [key, value] of Object.entries(passwordSettingsFormTruthTable)) {
        allow *= value;
    }
    if(allow)passwordSettingsForm.submit();
});