let selectedComponents = [];
let totalPrice = 0;

document.querySelectorAll('.component').forEach(component => {
    component.addEventListener('click', () => {
        const name = component.getAttribute('data-name');
        const price = parseInt(component.getAttribute('data-price'));

        if (component.classList.contains('selected')) {
            totalPrice -= price;
            selectedComponents = selectedComponents.filter(item => item.name !== name);
            component.classList.remove('selected');
        } else {
            const existingComponent = document.querySelector(`.component[data-name="${name}"].selected`);
            
            if (existingComponent) {
                alert(`Már ki van választva egy ${name}! Először töröld a korábban kiválasztottat.`);
                return;
            }

            totalPrice += price;
            selectedComponents.push({ name, price });
            component.classList.add('selected');
        }

        updateSelection();
    });
});

function updateSelection() {
    const selectedList = document.getElementById('selected-components');
    selectedList.innerHTML = '';
    selectedComponents.forEach(component => {
        const li = document.createElement('li');
        li.textContent = `${component.name}: ${component.price} Ft`;
        selectedList.appendChild(li);
    });
    document.getElementById('total-price').textContent = totalPrice;
}

function placeOrder() {
    const requiredComponents = ['ccase', 'Alaplap', 'CPU', 'Memória', 'Hattertar', 'monitor']; // További szükséges alkatrészek hozzáadása
    for (const component of requiredComponents) {
        if (!selectedComponents.some(item => item.name === component)) {
            alert(`Hiányzik: ${component}`);
            return;
        }
    }
    alert('Megrendelés leadva!');
}

let ratings = JSON.parse(localStorage.getItem('ratings')) || [];

function addRating() {
    const ratingValue = document.getElementById('rating-select').value;
    const comment = document.getElementById('rating-comment').value;
    
    if (!comment) {
        alert('Kérlek írj véleményt is!');
        return;
    }

    const rating = {
        stars: parseInt(ratingValue),
        comment: comment,
        date: new Date().toLocaleString()
    };

    ratings.unshift(rating);
    localStorage.setItem('ratings', JSON.stringify(ratings));
    
    document.getElementById('rating-comment').value = '';
    updateRatingsList();
}

function updateRatingsList() {
    const ratingsList = document.getElementById('ratings-list');
    ratingsList.innerHTML = '';

    ratings.forEach(rating => {
        const ratingElement = document.createElement('div');
        ratingElement.className = 'rating-item';
        
        const stars = '★'.repeat(rating.stars) + '☆'.repeat(5 - rating.stars);
        
        ratingElement.innerHTML = `
            <div class="rating-line">
                <span class="rating-stars">${stars}</span>
            </div>
            <div class="rating-comment">${rating.comment}</div>
            <div class="rating-date">${rating.date}</div>
        `;
        
        ratingsList.appendChild(ratingElement);
    });
}
function clearRatings() {
    ratings = [];
    localStorage.removeItem('ratings');
    updateRatingsList();
}

function setupGamerConfig() {
    clearSelections();
    
    const gamerSetup = {
        ccase: 4,      // High-end case
        Alaplap: 4,    // Best motherboard
        CPU: 4,        // Xeon processor
        Memória: 4,    // High-end memory
        GPU: 4,        // Best GPU
        Hattertar: 4,   // Fast storage
        monitor: 1,    // 4K monitor
        eger: 3       // 2000 dpi mouse
    };
    
    applySetup(gamerSetup);
}

function setupOfficeConfig() {
    clearSelections();
    
    const officeSetup = {
        ccase: 1,      // Basic case
        Alaplap: 1,    // normal motherboard
        CPU: 3,        // i5 processor
        Memória: 1,    // 8GB memory
        GPU: 1,        // Basic GPU
        Hattertar: 1,   // Basic storage
        monitor: 2,    // 24 inch monitor
        eger: 2,      // 1000 dpi mouse
    };
    
    applySetup(officeSetup);
}

function clearSelections() {
    document.querySelectorAll('.component.selected').forEach(comp => {
        comp.classList.remove('selected');
    });
    selectedComponents = [];
    totalPrice = 0;
    updateSelection();
}

function applySetup(setup) {
    Object.entries(setup).forEach(([type, id]) => {
        const component = document.querySelector(`.component[data-name="${type}"][data-id="${id}"]`);
        if (component) {
            component.click();
        }
    });
}
