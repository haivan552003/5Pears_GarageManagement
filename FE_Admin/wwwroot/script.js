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
    currentFacingMode = currentFacingMode === "user" ? "environment" : "user";
    window.startCamera();
};

// Capture image function
window.captureImage = async function () {
    const video = document.getElementById('cameraVideo');
    const canvas = document.getElementById('captureCanvas');
    const ctx = canvas.getContext('2d');

    if (!video || !canvas) {
        alert("Không tìm thấy phần tử video hoặc canvas.");
        return;
    }

    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

    const imageData = canvas.toDataURL('image/png');
    const imgElement = document.getElementById('capturedImage');

    imgElement.src = imageData;
    imgElement.style.display = "block";

    try {
        // Lưu ảnh vào localStorage
        localStorage.setItem('imageInput', imageData); // Lưu ảnh dưới dạng base64
    } catch (err) {
        console.error("Lỗi lưu ảnh vào localStorage: ", err);
    }
};

window.displayCapturedImage = function () {
    const imageUrl = localStorage.getItem('capturedImageUrl');
    const imgElement = document.getElementById('capturedImage');

    if (imageUrl && imgElement) {
        imgElement.src = imageUrl;
        imgElement.style.display = "block";
    } else {
        console.warn("Không tìm thấy URL ảnh đã chụp.");
    }
};

// Stop camera function
window.stopCameraAndHide = function () {
    const videoElement = document.getElementById('cameraVideo');

    if (window.currentStream) {
        const tracks = window.currentStream.getTracks();
        tracks.forEach(track => track.stop());
        window.currentStream = null;
    }

    if (videoElement) {
        videoElement.srcObject = null;
        videoElement.style.display = "none";
    }

};

window.downloadImage = function () {
    const downloadButton = document.getElementById('downloadButton');
    if (downloadButton && window.imageData) {
        const link = document.createElement('a');
        link.href = window.imageData;
        link.download = 'captured-image.png'; // Đặt tên file tải xuống
        link.click(); // Bắt đầu tải ảnh
    }
};


window.readTextFromImage = async function (fileInputId) {
    const inputElement = document.getElementById(fileInputId);

    // Kiểm tra nếu có file được chọn từ input
    if (inputElement.files && inputElement.files[0]) {
        try {
            // Nhận diện văn bản từ file đã chọn
            const result = await Tesseract.recognize(
                inputElement.files[0], // Sử dụng file đã chọn
                'eng',
                { logger: m => console.log(m) }
            );

            const text = result.data.text.trim();
            return text.length >= 5 ? text : "Không nhận diện được";
        } catch (error) {
            console.error("OCR Error:", error);
            return "Không nhận diện được";
        }
    }

    // Kiểm tra nếu có ảnh trong localStorage, nếu có thì sử dụng
    const imageData = localStorage.getItem('imageInput');
    if (imageData) {
        try {
            // Chuyển base64 thành ảnh
            const img = new Image();
            img.src = imageData;

            // Đợi ảnh tải xong trước khi xử lý OCR
            return new Promise((resolve, reject) => {
                img.onload = async function () {
                    try {
                        const result = await Tesseract.recognize(
                            img,
                            'eng',
                            { logger: m => console.log(m) }
                        );

                        const text = result.data.text.trim();
                        console.log("Nhận diện văn bản từ localStorage: ", text);

                        resolve(text.length >= 5 ? text : "Không nhận diện được");
                    } catch (error) {
                        console.error("OCR Error:", error);
                        reject("Không nhận diện được");
                    }
                };

                img.onerror = function () {
                    reject("Lỗi khi tải ảnh từ localStorage");
                };
            });
        } catch (error) {
            console.error("Lỗi xử lý ảnh từ localStorage:", error);
            return "Không nhận diện được";
        }
    } else {
        return "Không có ảnh hoặc file để nhận diện";
    }
};


window.triggerClick = function (elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.click();
    }
};

window.scanCapturedImage = async function (resultInputId) {
    const imageBase64 = localStorage.getItem('capturedImage');
    if (!imageBase64) {
        alert("Không tìm thấy ảnh để quét OCR.");
        return;
    }

    try {
        const result = await Tesseract.recognize(
            imageBase64,
            'eng',
            { logger: m => console.log(m) }
        );

        const text = result.data.text.trim();
        const resultInput = document.getElementById(resultInputId);

        if (resultInput) {
            resultInput.value = text;
            console.log("Kết quả OCR: ", text);
        } else {
            console.error("Không tìm thấy phần tử đầu vào để hiển thị kết quả OCR.");
        }
    } catch (error) {
        console.error("Lỗi OCR: ", error);
        alert("Không thể nhận diện văn bản từ ảnh. Vui lòng thử lại.");
    }
};
