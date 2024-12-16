// Swiper Slider Ayarları
var swiper = new Swiper('.swiper', {
    slidesPerView: 3, // Aynı anda 3 resmi yan yana göster
    spaceBetween: 10, // Resimler arasında boşluk
    navigation: {
        nextEl: '.swiper-button-next', // Sağ ok düğmesi
        prevEl: '.swiper-button-prev', // Sol ok düğmesi
    },
    pagination: {
        el: '.swiper-pagination', // Slider altındaki nokta navigasyonu
        clickable: true, // Noktalara tıklanabilirlik
    },
    loop: true, // Döngüsel kaydırma
    autoplay: {
        delay: 3000, // 3 saniyede bir otomatik kaydır
        disableOnInteraction: false, // Kullanıcı müdahale ettiğinde durmasın
    },
    breakpoints: {
        // Responsive ayarlar
        1024: {
            slidesPerView: 3, // Masaüstü için 3 resim
            spaceBetween: 20, // Masaüstü için boşluk
        },
        768: {
            slidesPerView: 2, // Tablet için 2 resim
            spaceBetween: 15, // Tablet için boşluk
        },
        480: {
            slidesPerView: 1, // Mobil için 1 resim
            spaceBetween: 10, // Mobil için boşluk
        },
    },
});

// Tüm Fotoğrafları Göster Butonu
function showAllPhotos() {
    document.getElementById("photo-modal").style.display = "flex";
}

// Modalı Kapatma Fonksiyonu
function closeModal() {
    document.getElementById("photo-modal").style.display = "none";
}

// Modal dışında tıklanınca kapatma
window.onclick = function (event) {
    const modal = document.getElementById("photo-modal");
    if (event.target === modal) {
        modal.style.display = "none";
    }
};

// Tarih Seçimi ve Hesaplama
flatpickr("#calendar1", {
    mode: "range",
    inline: true,
    dateFormat: "Y-m-d",
    minDate: "today",
    onChange: function (selectedDates) {
        if (selectedDates.length === 2) {
            const checkinDate = selectedDates[0].toISOString().split('T')[0];
            const checkoutDate = selectedDates[1].toISOString().split('T')[0];

            document.getElementById('checkin').value = checkinDate;
            document.getElementById('checkout').value = checkoutDate;

            calculateTotalPrice(selectedDates[0], selectedDates[1]);
        }
    }
});

function calculateTotalPrice(checkin, checkout) {
    const nights = Math.ceil((checkout - checkin) / (1000 * 60 * 60 * 24));
    const pricePerNight = parseFloat(document.querySelector(".reservation-panel h3").textContent.replace("₺ / gece", "").trim().replace(",", ""));
    const totalPrice = nights * pricePerNight;

    document.getElementById('total-nights').innerText = nights;
    document.getElementById('total-price').innerText = totalPrice.toLocaleString('tr-TR') + " ₺";
}