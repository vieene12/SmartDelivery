/* ==========================================================================
   ✨ PREMIUM ACTIONS & CALCULATORS BY ANTIGRAVITY (2026)
   ========================================================================== */

document.addEventListener('DOMContentLoaded', function () {
    // --- 1. DARK MODE THEME CONTROLLER ---
    const themeToggleBtn = document.getElementById('theme-toggle');
    
    // Sync UI with current theme on page load (applied in _Layout.cshtml head to avoid flicker)
    function syncThemeUI() {
        const isDark = document.body.classList.contains('dark-mode');
        if (themeToggleBtn) {
            const icon = themeToggleBtn.querySelector('i');
            if (icon) {
                if (isDark) {
                    icon.className = 'fas fa-sun';
                } else {
                    icon.className = 'fas fa-moon';
                }
            }
        }
    }
    
    // Initial sync
    syncThemeUI();

    // Theme toggle click handler
    if (themeToggleBtn) {
        themeToggleBtn.addEventListener('click', function () {
            document.body.classList.toggle('dark-mode');
            const isDark = document.body.classList.contains('dark-mode');
            localStorage.setItem('theme', isDark ? 'dark' : 'light');
            syncThemeUI();
        });
    }

    // --- 2. DYNAMIC HOMEPAGE ESTIMATOR (COUNT-UP) ---
    const homeWeightSlider = document.getElementById('home-weight-slider');
    const homeWeightVal = document.getElementById('home-weight-val');
    const routeBtns = document.querySelectorAll('.route-btn');
    const resultPrice = document.getElementById('home-result-price');
    const basePriceDisp = document.getElementById('home-base-price');
    const overweightPriceDisp = document.getElementById('home-overweight-price');

    let selectedRoute = 'hcm-hn';
    let currentPriceValue = 25000;

    // Route button toggles
    routeBtns.forEach(btn => {
        btn.addEventListener('click', function () {
            routeBtns.forEach(b => b.classList.remove('active'));
            this.classList.add('active');
            selectedRoute = this.dataset.route;
            calculateHomeFee();
        });
    });

    // Slider input handler
    if (homeWeightSlider) {
        homeWeightSlider.addEventListener('input', function () {
            if (homeWeightVal) {
                homeWeightVal.innerText = parseFloat(this.value).toFixed(1);
            }
            calculateHomeFee();
        });
    }

    // Count Up Animator Helper for Premium feel
    function animateCount(element, start, end, duration) {
        if (!element) return;
        if (start === end) {
            element.innerText = new Intl.NumberFormat('vi-VN').format(end);
            return;
        }
        let startTimestamp = null;
        const step = (timestamp) => {
            if (!startTimestamp) startTimestamp = timestamp;
            const progress = Math.min((timestamp - startTimestamp) / duration, 1);
            const currentValue = Math.floor(progress * (end - start) + start);
            element.innerText = new Intl.NumberFormat('vi-VN').format(currentValue);
            if (progress < 1) {
                window.requestAnimationFrame(step);
            } else {
                element.innerText = new Intl.NumberFormat('vi-VN').format(end);
            }
        };
        window.requestAnimationFrame(step);
    }

    function calculateHomeFee() {
        if (!homeWeightSlider) return;
        const weight = parseFloat(homeWeightSlider.value);
        
        let baseFee = selectedRoute === 'hcm-hn' ? 25000 : 29000;
        let extraFee = 0;

        if (weight > 3.0) {
            const extraWeight = weight - 3.0;
            const segments = Math.ceil(extraWeight / 0.5);
            extraFee = segments * 5000;
        }

        const totalFee = baseFee + extraFee;

        // Animate total price count up
        animateCount(resultPrice, currentPriceValue, totalFee, 250);
        currentPriceValue = totalFee;

        // Standard labels
        const formatter = new Intl.NumberFormat('vi-VN');
        if (basePriceDisp) basePriceDisp.innerText = formatter.format(baseFee) + 'đ';
        if (overweightPriceDisp) overweightPriceDisp.innerText = formatter.format(extraFee) + 'đ';
    }

    // Initial estimation run
    if (homeWeightSlider) {
        calculateHomeFee();
    }

    // --- 3. DYNAMIC INTERACTIVE SHIPMENT TRACKER ---
    const searchBtn = document.getElementById('btn-track');
    const searchInput = document.getElementById('input-track');
    const resultContainer = document.getElementById('tracking-result-container');

    if (searchBtn && searchInput) {
        searchBtn.addEventListener('click', function (e) {
            e.preventDefault();
            triggerTracking();
        });

        // Also trigger on enter key
        searchInput.addEventListener('keypress', function (e) {
            if (e.key === 'Enter') {
                e.preventDefault();
                triggerTracking();
            }
        });
    }

    function triggerTracking() {
        const trackingCode = searchInput.value.trim().toUpperCase();
        if (!trackingCode) {
            alert('Vui lòng nhập mã vận đơn để thực hiện tra cứu.');
            return;
        }

        // Display shipping card container
        resultContainer.style.display = 'block';
        resultContainer.scrollIntoView({ behavior: 'smooth', block: 'nearest' });

        // Let's generate tracking information dynamically
        const codeDisplay = document.getElementById('track-code-disp');
        if (codeDisplay) codeDisplay.innerText = trackingCode;

        // Mock state configurations depending on the input code
        let completedSteps = 2; // Default to processed
        let progressPercent = 66; // Stepper path length
        let truckPosition = 66; // Truck progress bar width
        let statusText = "Đang vận chuyển liên tỉnh";
        let statusBadgeClass = "badge-info";

        if (trackingCode.includes('8291') || trackingCode.includes('DELIVERED')) {
            completedSteps = 4;
            progressPercent = 100;
            truckPosition = 100;
            statusText = "Đã giao hàng thành công";
            statusBadgeClass = "badge-success";
        } else if (trackingCode.includes('NEW') || trackingCode.includes('8290') || trackingCode.length < 5) {
            completedSteps = 1;
            progressPercent = 0;
            truckPosition = 0;
            statusText = "Đã tiếp nhận yêu cầu";
            statusBadgeClass = "badge-warning";
        }

        // Update overall status badge
        const badgeElement = document.getElementById('track-status-badge');
        if (badgeElement) {
            badgeElement.className = `badge ${statusBadgeClass}`;
            badgeElement.innerHTML = `<i class="fas fa-circle-notch fa-spin"></i> ${statusText}`;
            if (completedSteps === 4) {
                badgeElement.innerHTML = `<i class="fas fa-check-circle"></i> ${statusText}`;
            }
        }

        // Animate Timeline progress
        const progressBar = document.querySelector('.stepper-progress-line');
        const truckProgressBar = document.querySelector('.truck-line-progress');
        const truckIcon = document.querySelector('.truck-icon-anim');
        const steps = document.querySelectorAll('.step-item');

        // Reset first to allow re-trigger animation
        if (progressBar) progressBar.style.height = '0%';
        if (truckProgressBar) truckProgressBar.style.width = '0%';
        if (truckIcon) truckIcon.style.left = '0%';
        
        steps.forEach(step => {
            step.classList.remove('completed', 'active');
            const pulse = step.querySelector('.step-marker-pulse');
            if (pulse) pulse.remove();
        });

        // Trigger reflow to restart transition
        setTimeout(() => {
            // Animate progress line heights
            if (progressBar) progressBar.style.height = `${progressPercent}%`;
            if (truckProgressBar) truckProgressBar.style.width = `${truckPosition}%`;
            if (truckIcon) truckIcon.style.left = `${truckPosition}%`;

            // Active step logic
            steps.forEach((step, idx) => {
                const stepNum = idx + 1;
                if (stepNum < completedSteps) {
                    step.classList.add('completed');
                } else if (stepNum === completedSteps) {
                    step.classList.add('active');
                    // Inject heartbeat radar ripple ring
                    const marker = step.querySelector('.step-marker');
                    if (marker && !marker.querySelector('.step-marker-pulse')) {
                        const pulseDiv = document.createElement('div');
                        pulseDiv.className = 'step-marker-pulse';
                        marker.appendChild(pulseDiv);
                    }
                }
            });
        }, 150);
    }
});
