import { initializeApp } from "firebase/app";
import { getAuth, RecaptchaVerifier, signInWithPhoneNumber } from "firebase/auth";

const firebaseConfig = {
    apiKey: "API_KEY",
    authDomain: "PROJECT_ID.firebaseapp.com",
    projectId: "PROJECT_ID",
    storageBucket: "PROJECT_ID.appspot.com",
    messagingSenderId: "SENDER_ID",
    appId: "APP_ID",
    measurementId: "G-MEASUREMENT_ID"
};

const app = initializeApp(firebaseConfig);
const auth = getAuth(app);

window.recaptchaVerifier = new RecaptchaVerifier('recaptcha-container', {
    size: 'invisible',
    callback: (response) => {
        // reCAPTCHA đã được giải quyết, có thể gửi OTP
    }
}, auth);

function sendOTP(phoneNumber) {
    const appVerifier = window.recaptchaVerifier;
    signInWithPhoneNumber(auth, phoneNumber, appVerifier)
        .then((confirmationResult) => {
            window.confirmationResult = confirmationResult;
        }).catch((error) => {
            console.error("Có lỗi xảy ra khi gửi mã OTP", error);
        });
}

function verifyOTP(otp) {
    const confirmationResult = window.confirmationResult;
    confirmationResult.confirm(otp).then((result) => {
        const user = result.user;
        console.log("Đăng nhập thành công với UID:", user.uid);
    }).catch((error) => {
        console.error("Xác thực OTP thất bại", error);
    });
}
