window.addEventListener('scroll', function () {
    const reservationPanel = document.querySelector('.reservation-panel');
    const scrollY = window.scrollY;

    if (scrollY > 100) {
        reservationPanel.classList.add('sticky');
    } else {
        reservationPanel.classList.remove('sticky');
    }
});
