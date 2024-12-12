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
        // Gọi OCR với Tesseract.js
        const result = await Tesseract.recognize(
            imageBase64,
            'eng',
            { logger: m => console.log(m) }
        );

        // Lấy văn bản từ OCR và loại bỏ tất cả các ký tự không hợp lệ
        let text = result.data.text.trim();

        // Loại bỏ tất cả các ký tự không hợp lệ, chỉ giữ lại ký tự hợp lệ
        text = text.replace(/[^A-Z0-9-\n]/g, ''); // Chỉ giữ lại chữ cái, số, và dấu '-'

        // Tách từng dòng (trong trường hợp kết quả nhiều dòng)
        const lines = text.split("\n").map(line => line.trim());

        // Regex kiểm tra biển số Việt Nam hợp lệ
        const plateRegex = /^[0-9]{2}[A-Z]{1}-[0-9]{3}\.?[0-9]{2}$/; // Ví dụ: 99A-123.45

        // Tìm dòng khớp với regex
        let plateNumber = lines.find(line => plateRegex.test(line));

        // Kiểm tra lại kết quả để đảm bảo nó là biển số hợp lệ
        if (plateNumber) {
            // Kiểm tra lần nữa nếu có ký tự lạ hoặc định dạng không phù hợp
            plateNumber = convertToValidLicensePlate(plateNumber); // Sử dụng hàm convert để chuẩn hóa biển số
        } else {
            plateNumber = "Không nhận diện được biển số hợp lệ"; // Nếu không tìm thấy biển số hợp lệ
        }

        // Hiển thị kết quả vào input
        const resultInput = document.getElementById(resultInputId);
        if (resultInput) {
            resultInput.value = plateNumber;
            console.log("Kết quả OCR: ", plateNumber);
        } else {
            console.error("Không tìm thấy phần tử đầu vào để hiển thị kết quả OCR.");
        }
    } catch (error) {
        console.error("Lỗi OCR: ", error);
        alert("Không thể nhận diện văn bản từ ảnh. Vui lòng thử lại.");
    }
};

// Hàm convert biển số hợp lệ
function convertToValidLicensePlate(input) {
    // Định nghĩa các ký tự hợp lệ cho biển số xe Việt Nam
    const validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    // Chuyển các ký tự đặc biệt trong tiếng Việt thành các ký tự tương ứng
    const vietnameseToLatinMap = {
        'à': 'a', 'á': 'a', 'ả': 'a', 'ã': 'a', 'ạ': 'a',
        'ă': 'a', 'ắ': 'a', 'ằ': 'a', 'ẳ': 'a', 'ẵ': 'a', 'ặ': 'a',
        'â': 'a', 'ấ': 'a', 'ầ': 'a', 'ẩ': 'a', 'ẫ': 'a', 'ậ': 'a',
        'è': 'e', 'é': 'e', 'ẻ': 'e', 'ẽ': 'e', 'ẹ': 'e',
        'ê': 'e', 'ế': 'e', 'ề': 'e', 'ể': 'e', 'ễ': 'e', 'ệ': 'e',
        'ì': 'i', 'í': 'i', 'ỉ': 'i', 'ĩ': 'i', 'ị': 'i',
        'ò': 'o', 'ó': 'o', 'ỏ': 'o', 'õ': 'o', 'ọ': 'o',
        'ô': 'o', 'ố': 'o', 'ồ': 'o', 'ổ': 'o', 'ỗ': 'o', 'ộ': 'o',
        'ơ': 'o', 'ớ': 'o', 'ờ': 'o', 'ở': 'o', 'ỡ': 'o', 'ợ': 'o',
        'ù': 'u', 'ú': 'u', 'ủ': 'u', 'ũ': 'u', 'ụ': 'u',
        'ư': 'u', 'ứ': 'u', 'ừ': 'u', 'ử': 'u', 'ữ': 'u', 'ự': 'u',
        'ỳ': 'y', 'ý': 'y', 'ỷ': 'y', 'ỹ': 'y', 'ỵ': 'y',
        'đ': 'd'
    };

    // Chuyển đổi ký tự tiếng Việt thành ký tự tương ứng
    input = input.split('').map(char => vietnameseToLatinMap[char] || char).join('');

    // Lọc ra chỉ những ký tự hợp lệ
    let filteredInput = '';
    for (let i = 0; i < input.length; i++) {
        if (validChars.includes(input[i].toUpperCase())) {
            filteredInput += input[i].toUpperCase();
        }
    }

    // Đảm bảo đầu ra theo định dạng biển số xe Việt Nam: "XX-123.41"
    let formatted = '';

    // Phần mã tỉnh và chữ cái đầu tiên
    formatted += filteredInput.substring(0, 2) + '-';

    // Phần số
    formatted += filteredInput.substring(2, 5);

    // Nếu có phần số thập phân, thêm phần này vào
    if (filteredInput.length > 5) {
        formatted += '.' + filteredInput.substring(5, 7);
    }

    return formatted;
}

