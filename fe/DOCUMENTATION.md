# Dokumentacja Frontend - System Rekomendacji Produktów

## 📋 Spis treści

1. [Przegląd projektu](#przegląd-projektu)
2. [Architektura systemu](#architektura-systemu)
3. [Struktura katalogów](#struktura-katalogów)
4. [Komponenty i funkcjonalności](#komponenty-i-funkcjonalności)
5. [Integracja z backendem](#integracja-z-backendem)
6. [Styling i CSS](#styling-i-css)
7. [Zarządzanie stanem](#zarządzanie-stanem)
8. [Routing](#routing)
9. [Obsługa błędów](#obsługa-błędów)
10. [Propozycje ulepszeń](#propozycje-ulepszeń)
11. [Standardy i best practices](#standardy-i-best-practices)

---

## 🎯 Przegląd projektu

### Cel projektu

Frontend aplikacji e-commerce z systemem rekomendacji produktów, zbudowany w React z TypeScript, wykorzystujący nowoczesne technologie i wzorce projektowe.

### Technologie

- **React 18** - biblioteka UI
- **TypeScript** - typowanie statyczne
- **Vite** - bundler i dev server
- **Tailwind CSS** - framework CSS
- **React Router** - routing
- **Lucide React** - ikony
- **Axios** - HTTP client

### Funkcjonalności główne

- Przeglądanie produktów z kategoriami
- System koszyka zakupów
- Autentykacja użytkowników
- System rekomendacji
- Responsywny design
- Dark/Light mode

---

## 🏗️ Architektura systemu

### Wzorzec architektoniczny

Projekt wykorzystuje **Clean Architecture** z podziałem na warstwy:

```
┌─────────────────────────────────────┐
│           Presentation Layer        │
│  (Components, Pages, Hooks)        │
├─────────────────────────────────────┤
│           Business Logic Layer      │
│  (Services, Context, Reducers)     │
├─────────────────────────────────────┤
│           Data Access Layer         │
│  (API Clients, HTTP Client)        │
├─────────────────────────────────────┤
│           External Layer            │
│  (Backend API, Local Storage)      │
└─────────────────────────────────────┘
```

### Zasady architektoniczne

1. **Separation of Concerns** - każda warstwa ma określoną odpowiedzialność
2. **Dependency Inversion** - warstwy wyższe nie zależą od niższych
3. **Single Responsibility** - każdy komponent ma jedną odpowiedzialność
4. **DRY (Don't Repeat Yourself)** - unikanie duplikacji kodu

---

## 📁 Struktura katalogów

```
src/
├── api/                    # Warstwa API
│   ├── client/            # HTTP client configuration
│   ├── authService.tsx    # Autentykacja
│   ├── cartService.tsx    # Operacje koszyka
│   └── productService.tsx # Operacje produktów
├── components/            # Komponenty UI
│   ├── common/           # Komponenty współdzielone
│   ├── ProductCard.tsx   # Karta produktu
│   └── ...
├── context/              # Zarządzanie stanem globalnym
│   ├── AppContext.tsx    # Główny context
│   ├── appReducer.ts     # Reducer dla stanu
│   └── useApp.ts         # Hook do contextu
├── hooks/                # Custom hooks
│   ├── useProducts.ts    # Hook dla produktów
│   └── useCart.ts        # Hook dla koszyka
├── pages/                # Strony aplikacji
│   ├── HomePage.tsx      # Strona główna
│   ├── ProductPage.tsx   # Szczegóły produktu
│   ├── CategoryPage.tsx  # Kategorie produktów
│   ├── CartPage.tsx      # Koszyk
│   └── ...
├── types/                # Definicje typów TypeScript
│   ├── api/             # Typy API
│   ├── authorization/   # Typy autentykacji
│   ├── cart/           # Typy koszyka
│   └── product/        # Typy produktów
├── services/            # Serwisy biznesowe
└── utils/               # Funkcje pomocnicze
```

---

## 🧩 Komponenty i funkcjonalności

### 1. Komponenty główne

#### ProductCard

**Lokalizacja**: `src/components/ProductCard.tsx`
**Funkcjonalność**: Wyświetlanie karty produktu z podstawowymi informacjami

**Props**:

```typescript
interface ProductCardProps {
  product: ProductDto;
}
```

**Funkcje**:

- Wyświetlanie obrazka produktu (placeholder)
- Informacje o produkcie (nazwa, cena, ocena)
- Badges (bestseller, nowość, promocja)
- Quick actions (dodaj do koszyka, ulubione)
- Responsywny design

**Klasy CSS**:

- `group` - hover effects
- `aspect-square` - proporcje obrazka
- `object-cover` - skalowanie obrazka
- `transition-*` - animacje

#### ProductPage

**Lokalizacja**: `src/pages/ProductPage.tsx`
**Funkcjonalność**: Szczegółowy widok produktu

**Funkcje**:

- Pełne informacje o produkcie
- Galeria obrazków
- Wybór rozmiaru i koloru
- Dodawanie do koszyka
- Informacje o dostawie i zwrotach

**Klasy CSS**:

- `grid grid-cols-1 lg:grid-cols-2` - layout dwukolumnowy
- `space-y-*` - spacing między elementami
- `rounded-lg` - zaokrąglone rogi

### 2. Strony aplikacji

#### HomePage

**Lokalizacja**: `src/pages/HomePage.tsx`
**Funkcjonalność**: Strona główna z sekcjami produktów

**Sekcje**:

- Hero section z call-to-action
- Bestsellery (4 produkty)
- Nowości (4 produkty)
- Polecane produkty (8 produktów)
- Newsletter signup

**Klasy CSS**:

- `bg-gradient-to-r` - gradient tła
- `max-w-7xl` - maksymalna szerokość
- `grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4` - responsive grid

#### CategoryPage

**Lokalizacja**: `src/pages/CategoryPage.tsx`
**Funkcjonalność**: Przeglądanie produktów według kategorii

**Funkcje**:

- Filtrowanie produktów
- Sortowanie (cena, ocena, nazwa)
- Paginacja
- Filtry zaawansowane (cena, rozmiar, kolor)

**Klasy CSS**:

- `flex gap-8` - layout z sidebar
- `w-64` - szerokość sidebara
- `grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4` - responsive grid

#### CartPage

**Lokalizacja**: `src/pages/CartPage.tsx`
**Funkcjonalność**: Zarządzanie koszykiem zakupów

**Funkcje**:

- Lista produktów w koszyku
- Zmiana ilości
- Usuwanie produktów
- Podsumowanie zamówienia
- Przejście do checkout

**Klasy CSS**:

- `flex items-center` - layout elementów
- `border-b` - separatory między elementami
- `rounded-lg shadow-sm` - karty produktów

### 3. Custom Hooks

#### useProducts

**Lokalizacja**: `src/hooks/useProducts.ts`
**Funkcjonalność**: Zarządzanie stanem produktów

**Stan**:

```typescript
interface UseProductsReturn {
  products: ProductDto[];
  currentProduct: ProductDto | null;
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  loading: boolean;
  error: string | null;
  // ... kategorie
}
```

**Funkcje**:

- `getProducts()` - pobieranie produktów
- `getProductById(id)` - pobieranie pojedynczego produktu
- `getBestsellers()` - bestsellery
- `getNewProducts()` - nowe produkty
- `searchProducts(query)` - wyszukiwanie
- `loadCategories()` - kategorie

#### useCart

**Lokalizacja**: `src/hooks/useCart.ts`
**Funkcjonalność**: Zarządzanie koszykiem

**Funkcje**:

- `addToCart(product)` - dodawanie do koszyka
- `removeFromCart(productId)` - usuwanie
- `updateQuantity(productId, quantity)` - zmiana ilości
- `clearCart()` - czyszczenie koszyka
- `getCartTotal()` - suma koszyka

---

## 🔌 Integracja z backendem

### Endpointy API

#### Produkty

```
GET /dic/products                    # Wszystkie produkty
GET /dic/products/{id}              # Produkt po ID
GET /dic/products/{id}/full         # Pełny produkt z detalami
GET /dic/products/category/{cat}    # Produkty po kategorii
GET /dic/products/bestsellers       # Bestsellery
GET /dic/products/new               # Nowe produkty
GET /dic/products/search?query={q}  # Wyszukiwanie
```

#### Koszyk

```
POST /cart/addItem                  # Dodaj do koszyka
POST /cart/removeItem               # Usuń z koszyka
POST /cart/updateQuantity           # Zmień ilość
POST /cart/clearCart                # Wyczyść koszyk
GET /cart/user                      # Koszyk użytkownika
POST /cart/getCartItems             # Elementy koszyka
```

#### Autentykacja

```
POST /signIn                        # Logowanie
POST /signOut                       # Wylogowanie
POST /changePassword                # Zmiana hasła
POST /remindPassword                # Przypomnienie hasła
```

### Typy danych

#### ProductDto

```typescript
interface ProductDto {
  id: string;
  productDisplayName: string;
  brandName: string;
  subCategoryId: string;
  subCategoryName: string;
  articleTypeId: string;
  articleTypeName: string;
  baseColourId: string;
  baseColourName: string;
  price: number;
  originalPrice?: number;
  rating: number;
  reviews: number;
  isBestseller: boolean;
  isNew: boolean;
}
```

#### CartItem

```typescript
interface CartItem {
  product: Product;
  quantity: number;
  size: string;
  color: string;
}
```

### HTTP Client

**Lokalizacja**: `src/api/client/httpClient.tsx`

**Funkcje**:

- `get<T>(url)` - GET request
- `post<T>(url, data)` - POST request
- `put<T>(url, data)` - PUT request
- `delete<T>(url)` - DELETE request

**Konfiguracja**:

- Base URL z environment variables
- Interceptors dla błędów
- CORS configuration
- Timeout settings

---

## 🎨 Styling i CSS

### Tailwind CSS Framework

#### Konfiguracja

**Lokalizacja**: `tailwind.config.js`

```javascript
module.exports = {
  content: ["./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      colors: {
        primary: {
          50: "#eff6ff",
          600: "#2563eb",
          700: "#1d4ed8",
          // ...
        },
      },
    },
  },
  plugins: [],
  darkMode: "class",
};
```

#### System kolorów

- **Primary**: Niebieski (#2563eb) - główne akcje
- **Gray**: Skala szarości - tekst, tła
- **Red**: Błędy, usuwanie (#ef4444)
- **Green**: Sukces, nowości (#10b981)
- **Yellow**: Ostrzeżenia, oceny (#f59e0b)

#### Responsive Design

```css
/* Mobile First */
.grid-cols-1                    /* 1 kolumna na mobile */
sm:grid-cols-2                  /* 2 kolumny na tablet */
lg:grid-cols-4                  /* 4 kolumny na desktop */
xl:grid-cols-5                  /* 5 kolumn na duży ekran */
```

#### Dark Mode

```css
/* Light mode */
bg-white text-gray-900

/* Dark mode */
dark:bg-gray-900 dark:text-white
```

#### Komponenty CSS

##### Karty produktów

```css
.group relative bg-white dark:bg-gray-800
rounded-lg shadow-sm hover:shadow-md
transition-shadow duration-200 overflow-hidden
```

##### Przyciski

```css
bg-primary-600 hover:bg-primary-700
text-white py-3 px-6 rounded-lg
font-medium transition-colors
```

##### Inputy

```css
px-4 py-3 border border-gray-300
dark:border-gray-600 rounded-lg
focus:ring-2 focus:ring-primary-500
focus:border-transparent
```

#### Animacje i przejścia

- `transition-colors` - płynne zmiany kolorów
- `transition-shadow` - płynne zmiany cieni
- `transition-transform` - płynne transformacje
- `hover:scale-105` - powiększenie przy hover
- `group-hover:opacity-100` - pokazywanie elementów

---

## 🔄 Zarządzanie stanem

### Context API

**Lokalizacja**: `src/context/AppContext.tsx`

#### Stan aplikacji

```typescript
interface AppState {
  products: ProductDto[];
  cart: CartItem[];
  user: User | null;
  orders: Order[];
  theme: Theme;
  isAuthModalOpen: boolean;
  authMode: "login" | "register";
  isChangePasswordModalOpen: boolean;
  isRemindPasswordModalOpen: boolean;
  isResetPasswordModalOpen: boolean;
  resetPasswordEmail: string;
  toasts: ToastItem[];
}
```

#### Akcje

```typescript
type AppAction =
  | {
      type: "ADD_TO_CART";
      payload: { product: ProductDto; size: string; color: string };
    }
  | { type: "REMOVE_FROM_CART"; payload: string }
  | {
      type: "UPDATE_CART_QUANTITY";
      payload: { productId: string; quantity: number };
    }
  | { type: "CLEAR_CART" }
  | { type: "SET_USER"; payload: User | null }
  | { type: "SET_THEME"; payload: Theme }
  | { type: "TOGGLE_AUTH_MODAL"; payload?: "login" | "register" }
  | { type: "CLOSE_AUTH_MODAL" }
  | { type: "ADD_TOAST"; payload: ToastItem }
  | { type: "REMOVE_TOAST"; payload: string }
  | { type: "ADD_ORDER"; payload: Order };
```

#### Reducer

**Lokalizacja**: `src/context/appReducer.ts`

**Funkcje**:

- Obsługa akcji koszyka
- Zarządzanie użytkownikiem
- Przełączanie motywów
- Zarządzanie modalami
- System powiadomień

### Local State

- **useState** - stan lokalny komponentów
- **useEffect** - efekty uboczne
- **useCallback** - memoizacja funkcji
- **useMemo** - memoizacja wartości

---

## 🛣️ Routing

### React Router v6

**Konfiguracja**: `src/App.tsx`

#### Struktura routingu

```typescript
<Routes>
  <Route path="/" element={<HomePage />} />
  <Route path="/category/:category" element={<CategoryPage />} />
  <Route path="/product/:id" element={<ProductPage />} />
  <Route path="/cart" element={<CartPage />} />
  <Route path="/checkout" element={<CheckoutPage />} />
  <Route path="/orders" element={<OrdersPage />} />
  <Route path="/bestsellers" element={<BestsellersPage />} />
  <Route path="/new" element={<NewProductsPage />} />
</Routes>
```

#### Parametry URL

- `:category` - kategoria produktów
- `:id` - ID produktu
- Query parameters - filtry, sortowanie

#### Navigation

- **Link** - nawigacja deklaratywna
- **useNavigate** - nawigacja programatyczna
- **useParams** - dostęp do parametrów URL

---

## ⚠️ Obsługa błędów

### Strategie obsługi błędów

#### 1. HTTP Errors

```typescript
// Interceptor w httpClient
axios.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      // Redirect to login
    } else if (error.response?.status === 404) {
      // Show 404 page
    } else {
      // Show generic error
    }
    return Promise.reject(error);
  }
);
```

#### 2. Component Error Boundaries

```typescript
class ErrorBoundary extends React.Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false };
  }

  static getDerivedStateFromError(error) {
    return { hasError: true };
  }

  componentDidCatch(error, errorInfo) {
    console.error("Error caught by boundary:", error, errorInfo);
  }

  render() {
    if (this.state.hasError) {
      return <ErrorFallback />;
    }
    return this.props.children;
  }
}
```

#### 3. Toast Notifications

```typescript
// System powiadomień
dispatch({
  type: "ADD_TOAST",
  payload: {
    id: Date.now().toString(),
    type: "error",
    message: "Wystąpił błąd podczas pobierania danych",
    duration: 5000,
  },
});
```

#### 4. Loading States

```typescript
const [loading, setLoading] = useState(false);
const [error, setError] = useState<string | null>(null);

// W komponencie
{
  loading && <LoadingSpinner />;
}
{
  error && <ErrorMessage message={error} />;
}
{
  !loading && !error && <Content />;
}
```

---

## 🚀 Propozycje ulepszeń

### 1. Performance

#### Lazy Loading

```typescript
// Lazy loading komponentów
const ProductPage = lazy(() => import("./pages/ProductPage"));
const CategoryPage = lazy(() => import("./pages/CategoryPage"));

// Suspense wrapper
<Suspense fallback={<LoadingSpinner />}>
  <ProductPage />
</Suspense>;
```

#### Virtual Scrolling

```typescript
// Dla dużych list produktów
import { FixedSizeList as List } from "react-window";

const ProductList = ({ products }) => (
  <List
    height={600}
    itemCount={products.length}
    itemSize={200}
    itemData={products}
  >
    {ProductRow}
  </List>
);
```

#### Image Optimization

```typescript
// Lazy loading obrazków
<img
  loading="lazy"
  src={product.image}
  alt={product.name}
  className="w-full h-full object-cover"
/>

// WebP format support
<picture>
  <source srcSet={product.imageWebP} type="image/webp" />
  <img src={product.image} alt={product.name} />
</picture>
```

### 2. User Experience

#### Infinite Scroll

```typescript
const useInfiniteScroll = (callback: () => void) => {
  const observer = useRef<IntersectionObserver>();

  const lastElementRef = useCallback(
    (node: HTMLElement) => {
      if (observer.current) observer.current.disconnect();
      observer.current = new IntersectionObserver((entries) => {
        if (entries[0].isIntersecting) {
          callback();
        }
      });
      if (node) observer.current.observe(node);
    },
    [callback]
  );

  return lastElementRef;
};
```

#### Search with Debounce

```typescript
const useDebounce = (value: string, delay: number) => {
  const [debouncedValue, setDebouncedValue] = useState(value);

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedValue(value);
    }, delay);

    return () => {
      clearTimeout(handler);
    };
  }, [value, delay]);

  return debouncedValue;
};
```

#### Progressive Web App (PWA)

```typescript
// Service Worker
// manifest.json
// Offline support
// Push notifications
```

### 3. State Management

#### Zustand (alternatywa dla Context)

```typescript
import create from "zustand";

interface Store {
  products: ProductDto[];
  cart: CartItem[];
  addToCart: (product: ProductDto) => void;
  removeFromCart: (productId: string) => void;
}

const useStore = create<Store>((set) => ({
  products: [],
  cart: [],
  addToCart: (product) =>
    set((state) => ({
      cart: [...state.cart, { product, quantity: 1, size: "", color: "" }],
    })),
  removeFromCart: (productId) =>
    set((state) => ({
      cart: state.cart.filter((item) => item.product.id !== productId),
    })),
}));
```

#### React Query (dla cache'owania)

```typescript
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";

const useProducts = () => {
  return useQuery({
    queryKey: ["products"],
    queryFn: () => productService.getProducts(),
    staleTime: 5 * 60 * 1000, // 5 minut
    cacheTime: 10 * 60 * 1000, // 10 minut
  });
};
```

### 4. Testing

#### Jest + React Testing Library

```typescript
import { render, screen, fireEvent } from "@testing-library/react";
import { ProductCard } from "./ProductCard";

test("renders product card with correct information", () => {
  const product = mockProduct;
  render(<ProductCard product={product} />);

  expect(screen.getByText(product.productDisplayName)).toBeInTheDocument();
  expect(
    screen.getByText(`${product.price.toFixed(2)} zł`)
  ).toBeInTheDocument();
});

test("adds product to cart when button is clicked", () => {
  const mockDispatch = jest.fn();
  render(<ProductCard product={mockProduct} />);

  fireEvent.click(screen.getByText("Dodaj do koszyka"));
  expect(mockDispatch).toHaveBeenCalledWith({
    type: "ADD_TO_CART",
    payload: expect.objectContaining({
      product: mockProduct,
    }),
  });
});
```

#### Cypress (E2E Testing)

```typescript
describe("Product Flow", () => {
  it("should add product to cart", () => {
    cy.visit("/");
    cy.get('[data-testid="product-card"]').first().click();
    cy.get('[data-testid="add-to-cart"]').click();
    cy.get('[data-testid="cart-count"]').should("contain", "1");
  });
});
```

### 5. Accessibility

#### ARIA Labels

```typescript
<button
  aria-label="Dodaj produkt do koszyka"
  onClick={addToCart}
  className="..."
>
  <ShoppingCart className="w-5 h-5" />
</button>
```

#### Keyboard Navigation

```typescript
const handleKeyDown = (event: KeyboardEvent) => {
  if (event.key === "Enter" || event.key === " ") {
    event.preventDefault();
    addToCart();
  }
};
```

#### Screen Reader Support

```typescript
<div role="main" aria-label="Lista produktów">
  {products.map((product) => (
    <article key={product.id} role="article">
      <h2>{product.productDisplayName}</h2>
      <p aria-label={`Cena: ${product.price} złotych`}>
        {product.price.toFixed(2)} zł
      </p>
    </article>
  ))}
</div>
```

### 6. Internationalization (i18n)

#### React Intl

```typescript
import { IntlProvider, FormattedMessage, FormattedNumber } from "react-intl";

const messages = {
  "product.addToCart": "Dodaj do koszyka",
  "product.price": "{price} zł",
  "cart.empty": "Twój koszyk jest pusty",
};

<IntlProvider messages={messages} locale="pl">
  <App />
</IntlProvider>;
```

---

## 📋 Standardy i best practices

### 1. Code Style

#### TypeScript

- Strict mode enabled
- Explicit return types dla funkcji publicznych
- Interface over type dla obiektów
- Generic types dla komponentów wielokrotnego użytku

#### Naming Conventions

```typescript
// Komponenty - PascalCase
const ProductCard = () => {};

// Funkcje - camelCase
const addToCart = () => {};

// Stałe - UPPER_SNAKE_CASE
const API_BASE_URL = "https://api.example.com";

// Interfejsy - PascalCase z I prefix
interface IProductCardProps {}

// Typy - PascalCase
type CartAction = "ADD" | "REMOVE" | "UPDATE";
```

#### File Organization

```
ComponentName/
├── index.ts              # Export główny
├── ComponentName.tsx     # Komponent
├── ComponentName.test.tsx # Testy
├── ComponentName.styles.css # Style (jeśli potrzebne)
└── types.ts             # Typy lokalne
```

### 2. Performance

#### Bundle Analysis

```bash
npm run build -- --analyze
```

#### Code Splitting

```typescript
// Dynamic imports
const ProductPage = lazy(() => import("./pages/ProductPage"));

// Route-based splitting
const routes = [
  {
    path: "/product/:id",
    component: lazy(() => import("./pages/ProductPage")),
  },
];
```

#### Memoization

```typescript
// React.memo dla komponentów
const ProductCard = React.memo(({ product }: ProductCardProps) => {
  return <div>{product.name}</div>;
});

// useMemo dla obliczeń
const expensiveValue = useMemo(() => {
  return products.filter((p) => p.price > 100);
}, [products]);

// useCallback dla funkcji
const handleClick = useCallback(() => {
  addToCart(product);
}, [product, addToCart]);
```

### 3. Security

#### Input Validation

```typescript
const validateEmail = (email: string): boolean => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email);
};

const validatePassword = (password: string): boolean => {
  return (
    password.length >= 8 &&
    /[A-Z]/.test(password) &&
    /[a-z]/.test(password) &&
    /[0-9]/.test(password)
  );
};
```

#### XSS Prevention

```typescript
// Sanityzacja danych
import DOMPurify from "dompurify";

const sanitizedHtml = DOMPurify.sanitize(userInput);

// Bezpieczne renderowanie
<div dangerouslySetInnerHTML={{ __html: sanitizedHtml }} />;
```

#### CSRF Protection

```typescript
// Token w headerach
axios.defaults.headers.common["X-CSRF-Token"] = getCsrfToken();

// SameSite cookies
document.cookie = "sessionId=123; SameSite=Strict";
```

### 4. Monitoring

#### Error Tracking

```typescript
// Sentry integration
import * as Sentry from "@sentry/react";

Sentry.init({
  dsn: "YOUR_DSN",
  environment: process.env.NODE_ENV,
  integrations: [new Sentry.BrowserTracing()],
});

// Error boundary
<Sentry.ErrorBoundary fallback={<ErrorFallback />}>
  <App />
</Sentry.ErrorBoundary>;
```

#### Analytics

```typescript
// Google Analytics
import ReactGA from "react-ga";

ReactGA.initialize("GA_TRACKING_ID");

// Track page views
useEffect(() => {
  ReactGA.pageview(window.location.pathname);
}, [location]);

// Track events
const trackAddToCart = (product: ProductDto) => {
  ReactGA.event({
    category: "Ecommerce",
    action: "Add to Cart",
    label: product.productDisplayName,
    value: product.price,
  });
};
```

#### Performance Monitoring

```typescript
// Web Vitals
import { getCLS, getFID, getFCP, getLCP, getTTFB } from "web-vitals";

getCLS(console.log);
getFID(console.log);
getFCP(console.log);
getLCP(console.log);
getTTFB(console.log);
```

---

## 🔧 Konfiguracja środowiska

### Environment Variables

```bash
# .env.local
VITE_API_BASE_URL=http://localhost:5000
VITE_APP_NAME=Recommendations Frontend
VITE_GA_TRACKING_ID=GA-XXXXXXXXX
VITE_SENTRY_DSN=https://xxxxx@sentry.io/xxxxx
```

### Build Configuration

```typescript
// vite.config.ts
import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import { resolve } from "path";

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      "@": resolve(__dirname, "src"),
      "@components": resolve(__dirname, "src/components"),
      "@pages": resolve(__dirname, "src/pages"),
      "@hooks": resolve(__dirname, "src/hooks"),
      "@types": resolve(__dirname, "src/types"),
      "@api": resolve(__dirname, "src/api"),
    },
  },
  build: {
    outDir: "dist",
    sourcemap: true,
    rollupOptions: {
      output: {
        manualChunks: {
          vendor: ["react", "react-dom"],
          router: ["react-router-dom"],
          ui: ["lucide-react"],
        },
      },
    },
  },
});
```

### Package.json Scripts

```json
{
  "scripts": {
    "dev": "vite",
    "build": "tsc && vite build",
    "preview": "vite preview",
    "lint": "eslint . --ext ts,tsx --report-unused-disable-directives --max-warnings 0",
    "lint:fix": "eslint . --ext ts,tsx --fix",
    "type-check": "tsc --noEmit",
    "test": "jest",
    "test:watch": "jest --watch",
    "test:coverage": "jest --coverage",
    "e2e": "cypress run",
    "e2e:open": "cypress open"
  }
}
```

---

## 📊 Metryki i KPI

### Performance Metrics

- **First Contentful Paint (FCP)**: < 1.5s
- **Largest Contentful Paint (LCP)**: < 2.5s
- **First Input Delay (FID)**: < 100ms
- **Cumulative Layout Shift (CLS)**: < 0.1

### User Experience Metrics

- **Time to Interactive**: < 3s
- **Bundle Size**: < 500KB (gzipped)
- **Lighthouse Score**: > 90

### Business Metrics

- **Conversion Rate**: % użytkowników dodających do koszyka
- **Cart Abandonment Rate**: % porzuconych koszyków
- **Average Order Value**: średnia wartość zamówienia
- **User Engagement**: czas spędzony na stronie

---

## 🎯 Roadmap

### Phase 1 (Q1 2024)

- [ ] Implementacja PWA
- [ ] Dodanie testów jednostkowych
- [ ] Optymalizacja performance
- [ ] Dodanie dark mode

### Phase 2 (Q2 2024)

- [ ] Implementacja React Query
- [ ] Dodanie E2E testów
- [ ] Internationalization
- [ ] Advanced filtering

### Phase 3 (Q3 2024)

- [ ] Micro-frontends architecture
- [ ] Real-time features
- [ ] Advanced analytics
- [ ] A/B testing framework

### Phase 4 (Q4 2024)

- [ ] AI-powered recommendations
- [ ] Voice search
- [ ] AR product preview
- [ ] Social commerce features

---

## 📞 Wsparcie i kontakt

### Dokumentacja

- [React Documentation](https://react.dev/)
- [TypeScript Handbook](https://www.typescriptlang.org/docs/)
- [Tailwind CSS Docs](https://tailwindcss.com/docs)
- [Vite Guide](https://vitejs.dev/guide/)

### Narzędzia deweloperskie

- React Developer Tools
- Redux DevTools (dla Context)
- Tailwind CSS IntelliSense
- TypeScript Language Server

### Monitoring i debugging

- Browser DevTools
- Network tab dla API calls
- Console dla błędów
- Performance tab dla metryk

---

_Dokumentacja została wygenerowana automatycznie na podstawie analizy kodu źródłowego. Ostatnia aktualizacja: 2024-01-XX_
