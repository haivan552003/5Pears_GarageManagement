//FireBase 
// Import the functions you need from the SDKs you need
import { initializeApp } from "https://www.gstatic.com/firebasejs/10.14.0/firebase-app.js";
import { getAnalytics } from "https://www.gstatic.com/firebasejs/10.14.0/firebase-analytics.js";
import { getStorage, ref, uploadBytes, getDownloadURL } from "https://www.gstatic.com/firebasejs/10.14.0/firebase-storage.js";

// Your web app's Firebase configuration
const firebaseConfig = {
    apiKey: "AIzaSyDHguIZkgDiKR6_EtOeQOQij79t8tXZS0w",
    authDomain: "pear-b8fc3.firebaseapp.com",
    projectId: "pear-b8fc3",
    storageBucket: "pear-b8fc3.appspot.com",
    messagingSenderId: "663990872588",
    appId: "1:663990872588:web:e4b3b81a82e0bfb4c31c29",
    measurementId: "G-XFDQG449JZ"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const analytics = getAnalytics(app);
const storage = getStorage(app);

// Upload file
async function uploadImage(fileName, base64String) {
    const fileRef = ref(storage, fileName);
    try {
        const response = await fetch(`data:image/jpeg;base64,${base64String}`);
        const blob = await response.blob();
        await uploadBytes(fileRef, blob);
        const downloadURL = await getDownloadURL(fileRef);
        return downloadURL;
    } catch (error) {
        console.error("Error uploading image:", error);
        throw error;
    }
}


// Expose function to global window object
window.uploadImage = uploadImage;

async function getImageUrl(fileName) {
    const fileRef = ref(storage, fileName);
    try {
        const downloadURL = await getDownloadURL(fileRef);
        return downloadURL;
    } catch (error) {
        console.error("Error getting download URL:", error);
        throw error;
    }
}
