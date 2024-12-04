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
}// Expose function to global window object
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

window.readTextFromImage = async function (fileInputId, resultInputId) {
    const inputElement = document.getElementById(fileInputId);
    const resultElement = document.getElementById(resultInputId);

    if (inputElement.files && inputElement.files[0]) {
        const file = inputElement.files[0];
        const reader = new FileReader();

        reader.onload = async function () {
            const image = new Image();
            image.src = reader.result;

            image.onload = async function () {
                const canvas = document.createElement('canvas');
                canvas.width = image.width;
                canvas.height = image.height;
                const ctx = canvas.getContext('2d');
                ctx.drawImage(image, 0, 0);

                // Sử dụng thư viện OCR như Tesseract.js để nhận diện văn bản
                const result = await Tesseract.recognize(
                    canvas.toDataURL(),
                    'eng',
                    { logger: m => console.log(m) }
                );

                const text = result.text.trim();
                resultElement.value = text;

                // Lưu dữ liệu vào localStorage
                try {
                    localStorage.setItem("scannedText", text);
                    console.log("Dữ liệu được lưu vào localStorage:", text);
                } catch (error) {
                    console.error("Không thể lưu dữ liệu vào localStorage:", error);
                }
            };
        };

        reader.readAsDataURL(file);
    }
};







let currentFacingMode = "environment"; // Mặc định là camera sau

// Start camera function
window.startCamera = function () {
    const videoElement = document.getElementById('cameraVideo');

    // Dừng camera nếu đã bật
    if (window.currentStream) {
        const tracks = window.currentStream.getTracks();
        tracks.forEach(track => track.stop());
    }

    // Thiết lập camera với facingMode hiện tại
    navigator.mediaDevices.getUserMedia({
        video: { facingMode: currentFacingMode }
    }).then((stream) => {
        videoElement.srcObject = stream;
        videoElement.play();
        window.currentStream = stream; // Lưu stream để có thể dừng sau này
    }).catch((err) => {
        console.error("Error accessing camera: ", err);
    });
};

// Switch camera function
window.switchCamera = function () {
    currentFacingMode = currentFacingMode === "user" ? "environment" : "user"; // Chuyển đổi giữa camera trước và sau
    window.startCamera(); // Khởi động lại camera
};

// Capture image function
window.captureImage = function () {
    const video = document.getElementById('cameraVideo');
    const canvas = document.getElementById('captureCanvas');
    const ctx = canvas.getContext('2d');

    if (!canvas || !video) {
        console.error("Canvas or video element not found!");
        return;
    }

    // Set canvas kích thước giống như video
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;

    // Vẽ ảnh từ video lên canvas
    ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

    // Lấy dữ liệu ảnh từ canvas
    const imageData = canvas.toDataURL('image/png');

    // Tạo liên kết để tải hình ảnh
    const downloadLink = document.getElementById('downloadLink');
    if (downloadLink) {
        downloadLink.href = imageData;  
        downloadLink.download = 'captured-image.png'; // Đặt tên tệp
        downloadLink.style.display = 'inline';  
    }

    return imageData;
};


// Stop camera function
window.stopCamera = function () {
    const stream = window.currentStream;
    const tracks = stream.getTracks();
    tracks.forEach(track => track.stop());
};


window.readTextFromImage = function (imageInputId, resultInputId) {
    // Hàm này sẽ xử lý OCR từ hình ảnh
    const inputElement = document.getElementById(imageInputId);
    const resultElement = document.getElementById(resultInputId);

    // Ví dụ sử dụng Tesseract.js hoặc một thư viện OCR khác
    Tesseract.recognize(
        inputElement.files[0],
        'eng',
        {
            logger: m => console.log(m),
        }
    ).then(({ data: { text } }) => {
        resultElement.value = text;
    });
};
