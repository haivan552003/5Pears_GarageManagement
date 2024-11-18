// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getAnalytics } from "firebase/analytics";
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
    apiKey: "AIzaSyBMxGYR1UYvkfX_M7GU2XT0xGK_EQwziQQ",
    authDomain: "management-c91fe.firebaseapp.com",
    projectId: "management-c91fe",
    storageBucket: "management-c91fe.firebasestorage.app",
    messagingSenderId: "565045131532",
    appId: "1:565045131532:web:2960f59875e2190866f129",
    measurementId: "G-7C7TDVQXQ1"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const analytics = getAnalytics(app);