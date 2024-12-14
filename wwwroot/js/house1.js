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
