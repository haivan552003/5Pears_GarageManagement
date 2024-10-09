//FireBase 
// Import the functions you need from the SDKs you need
import { initializeApp } from "https://www.gstatic.com/firebasejs/10.13.0/firebase-app.js";
import { getAnalytics } from "https://www.gstatic.com/firebasejs/10.13.0/firebase-analytics.js";
import { getStorage, ref, uploadBytes, getDownloadURL } from "https://www.gstatic.com/firebasejs/10.13.0/firebase-storage.js";

// Your web app's Firebase configuration
const firebaseConfig = {
    apiKey: "AIzaSyCustmiBJwTBcvDC7Qr--r9ucj4tWkAlss",
    authDomain: "fivepeargragemangement.firebaseapp.com",
    projectId: "fivepeargragemangement",
    storageBucket: "fivepeargragemangement.appspot.com",
    messagingSenderId: "616799845315",
    appId: "1:616799845315:web:2acd3b8ae9bb2a38dd5b76",
    measurementId: "G-GEVQNM3B13"
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
