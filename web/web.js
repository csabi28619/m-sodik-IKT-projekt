const products = [
    {
        id: 1,
        name: "AMD Ryzen 5 5600X",
        category: "cpu",
        brand: "amd",
        shortDesc: "6 mag/12 szál, 3.7GHz",
        longDesc: "A Ryzen 5 5600X egy nagyteljesítményű processzor játékhoz és munkához...",
        price: 89990,
        specs: {
            cores: 6,
            threads: 12,
            baseSpeed: 3.7,
            maxSpeed: 4.6
        }
    },
    // Add more products here...
];

function initializePage() {
    displayProducts();
    populateCompareSelects();
    setupEventListeners();
}

function displayProducts() {
    const grid = document.querySelector('.products-grid');
    grid.innerHTML = products.map(product => `
        <div class="product-card ${product.brand}">
            <h3>${product.name}</h3>
            <p>${product.shortDesc}</p>
            <button onclick="toggleDetails(${product.id})">Részletek</button>
            <div class="product-details" id="details-${product.id}">
                <p>${product.longDesc}</p>
                <p>Ár: ${product.price} Ft</p>
            </div>
        </div>
    `).join('');
}

function toggleDetails(id) {
    const details = document.getElementById(`details-${id}`);
    details.style.display = details.style.display === 'none' ? 'block' : 'none';
}

function compareProducts() {
    const product1 = products.find(p => p.id === parseInt(document.getElementById('product1').value));
    const product2 = products.find(p => p.id === parseInt(document.getElementById('product2').value));
    
    const results = document.querySelector('.compare-results');
    results.innerHTML = generateComparisonHTML(product1, product2);
}

function handleSearch() {
    const searchTerm = document.getElementById('search').value.toLowerCase();
    const filteredProducts = products.filter(product => 
        product.name.toLowerCase().includes(searchTerm) ||
        product.shortDesc.toLowerCase().includes(searchTerm)
    );
    displayFilteredProducts(filteredProducts);
}

document.addEventListener('DOMContentLoaded', initializePage);
