import { useState, useCallback } from "react";
import * as productService from "../api/productService";
import type { ProductDto } from "../types/product/ProductDto";
import type {
  GetProductsResponse,
  GetProductByIdResponse,
  GetMasterCategoriesResponse,
  GetSubCategoriesResponse,
  GetArticleTypesResponse,
  GetBaseColoursResponse,
  SearchProductsResponse,
  GetProductsRequest,
} from "../types/product/ProductApi";
import type { MasterCategoryDto } from "../types/product/ProductDto";

interface UseProductsReturn {
  // Products state
  products: ProductDto[];
  currentProduct: ProductDto | null;
  totalProductCount: number;
  productPage: number;
  productPageSize: number;
  totalProductPages: number;
  loading: boolean;
  error: string | null;

  // Bestsellers state
  bestsellers: ProductDto[];
  bestsellersLoading: boolean;
  bestsellersError: string | null;
  totalBestsellersCount: number;
  bestsellersPage: number;
  bestsellersPageSize: number;
  totalBestsellersPages: number;

  // New products state
  newProducts: ProductDto[];
  newProductsLoading: boolean;
  newProductsError: string | null;
  totalNewProductsCount: number;
  newProductsPage: number;
  newProductsPageSize: number;
  totalNewProductsPages: number;

  // Categories state
  masterCategories: MasterCategoryDto[];
  subCategories: GetSubCategoriesResponse["subCategories"];
  articleTypes: GetArticleTypesResponse["articleTypes"];
  baseColours: GetBaseColoursResponse["baseColours"];
  categoriesLoading: boolean;
  categoriesError: string | null;

  // Actions
  getProducts: (pageNumber?: number, pageSizeNumber?: number) => Promise<void>;
  getProductById: (productId: string) => Promise<void>;
  getBestsellers: (
    pageNumber?: number,
    pageSizeNumber?: number
  ) => Promise<void>;
  getNewProducts: (
    pageNumber?: number,
    pageSizeNumber?: number
  ) => Promise<void>;
  getProductsByCategory: (
    masterCategoryId?: string,
    subCategoryId?: string,
    page?: number,
    pageSize?: number
  ) => Promise<void>;
  searchProducts: (query: string) => Promise<void>;
  loadCategories: () => Promise<void>;
  clearError: () => void;
  clearBestsellersError: () => void;
  clearNewProductsError: () => void;
  clearCategoriesError: () => void;
  setSubCategories: React.Dispatch<
    React.SetStateAction<GetSubCategoriesResponse["subCategories"]>
  >;
  setArticleTypes: React.Dispatch<
    React.SetStateAction<GetArticleTypesResponse["articleTypes"]>
  >;
  setBaseColours: React.Dispatch<
    React.SetStateAction<GetBaseColoursResponse["baseColours"]>
  >;
}

export const useProducts = (): UseProductsReturn => {
  // Products state
  const [products, setProducts] = useState<ProductDto[]>([]);
  const [currentProduct, setCurrentProduct] = useState<ProductDto | null>(null);
  const [totalProductCount, setProductTotalCount] = useState(0);
  const [productPage, setProductPage] = useState(1);
  const [productPageSize, setProductPageSize] = useState(20);
  const [totalProductPages, setProductTotalPages] = useState(0);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Bestsellers state
  const [bestsellers, setBestsellers] = useState<ProductDto[]>([]);
  const [bestsellersLoading, setBestsellersLoading] = useState(false);
  const [bestsellersError, setBestsellersError] = useState<string | null>(null);
  const [totalBestsellersCount, setBestsellersTotalCount] = useState(0);
  const [bestsellersPage, setBestsellersPage] = useState(1);
  const [bestsellersPageSize, setBestsellersPageSize] = useState(20);
  const [totalBestsellersPages, setBestsellersTotalPages] = useState(0);

  // New products state
  const [newProducts, setNewProducts] = useState<ProductDto[]>([]);
  const [newProductsLoading, setNewProductsLoading] = useState(false);
  const [newProductsError, setNewProductsError] = useState<string | null>(null);
  const [totalNewProductsCount, setNewProductsTotalCount] = useState(0);
  const [newProductsPage, setNewProductsPage] = useState(1);
  const [newProductsPageSize, setNewProductsPageSize] = useState(20);
  const [totalNewProductsPages, setNewProductsTotalPages] = useState(0);

  // Categories state
  const [masterCategories, setMasterCategories] = useState<
    GetMasterCategoriesResponse["masterCategories"]
  >([]);
  const [subCategories, setSubCategories] = useState<
    GetSubCategoriesResponse["subCategories"]
  >([]);
  const [articleTypes, setArticleTypes] = useState<
    GetArticleTypesResponse["articleTypes"]
  >([]);
  const [baseColours, setBaseColours] = useState<
    GetBaseColoursResponse["baseColours"]
  >([]);
  const [categoriesLoading, setCategoriesLoading] = useState(false);
  const [categoriesError, setCategoriesError] = useState<string | null>(null);

  const clearError = useCallback(() => {
    setError(null);
  }, []);

  const clearBestsellersError = useCallback(() => {
    setBestsellersError(null);
  }, []);

  const clearNewProductsError = useCallback(() => {
    setNewProductsError(null);
  }, []);

  const clearCategoriesError = useCallback(() => {
    setCategoriesError(null);
  }, []);

  const getProducts = useCallback(
    async (
      pageNumber: number = 1,
      pageSizeNumber: number = 20,
      filters: Partial<Omit<GetProductsRequest, "page" | "pageSize">> = {}
    ) => {
      setLoading(true);
      setError(null);

      try {
        // Składamy parametry zapytania
        const params: GetProductsRequest = {
          page: pageNumber,
          pageSize: pageSizeNumber,
          ...filters,
        };

        const result = await productService.getProducts(params);

        if (result.status === 200 && result.data) {
          const response: GetProductsResponse = result.data;
          setProducts(response.products);
          setProductTotalCount(response.totalCount);
          setProductPage(response.page);
          setProductPageSize(response.pageSize);
          setProductTotalPages(response.totalPages);
        } else {
          setError(result.message || "Nie udało się pobrać produktów");
        }
      } catch (err) {
        setError("Błąd podczas pobierania produktów");
        console.error("Error fetching products:", err);
      } finally {
        setLoading(false);
      }
    },
    []
  );

  const getProductById = useCallback(async (productId: string) => {
    setLoading(true);
    setError(null);

    try {
      const result = await productService.getProductById({ productId });

      if (result.status === 200 && result.data) {
        const response: GetProductByIdResponse = result.data;
        setCurrentProduct(response.product);
      } else {
        setError(result.message || "Nie udało się pobrać produktu");
      }
    } catch (err) {
      setError("Błąd podczas pobierania produktu");
      console.error("Error fetching product:", err);
    } finally {
      setLoading(false);
    }
  }, []);

  const getBestsellers = useCallback(
    async (
      pageNumber: number = 1,
      pageSizeNumber: number = 20,
      filters: Partial<Omit<GetProductsRequest, "page" | "pageSize">> = {}
    ) => {
      setBestsellersLoading(true);
      setBestsellersError(null);

      try {
        const params: GetProductsRequest = {
          page: pageNumber,
          pageSize: pageSizeNumber,
          ...filters,
        };
        const result = await productService.getBestsellers(params);
        if (result.status === 200 && result.data) {
          const response: GetProductsResponse = result.data;
          setBestsellersTotalCount(response.totalCount);
          setBestsellersPage(response.page);
          setBestsellersPageSize(response.pageSize);
          setBestsellersTotalPages(response.totalPages);
          setBestsellers(response.products);
        } else {
          setBestsellersError(
            result.message || "Nie udało się pobrać bestsellerów"
          );
        }
      } catch (err) {
        setBestsellersError("Błąd podczas pobierania bestsellerów");
        console.error("Error fetching bestsellers:", err);
      } finally {
        setBestsellersLoading(false);
      }
    },
    []
  );

  const getNewProducts = useCallback(
    async (
      pageNumber: number = 1,
      pageSizeNumber: number = 20,
      filters: Partial<Omit<GetProductsRequest, "page" | "pageSize">> = {}
    ) => {
      setNewProductsLoading(true);
      setNewProductsError(null);

      try {
        const params: GetProductsRequest = {
          page: pageNumber,
          pageSize: pageSizeNumber,
          ...filters,
        };

        const result = await productService.getNewProducts(params);

        if (result.status === 200 && result.data) {
          const response: GetProductsResponse = result.data;

          setNewProductsTotalCount(response.totalCount);
          setNewProductsPage(response.page);
          setNewProductsPageSize(response.pageSize);
          setNewProductsTotalPages(response.totalPages);
          setNewProducts(response.products);
        } else {
          setNewProductsError(
            result.message || "Nie udało się pobrać nowych produktów"
          );
        }
      } catch (err) {
        setNewProductsError("Błąd podczas pobierania nowych produktów");
        console.error("Error fetching new products:", err);
      } finally {
        setNewProductsLoading(false);
      }
    },
    []
  );

  const getProductsByCategory = useCallback(
    async (
      masterCategoryId?: string,
      subCategoryId?: string,
      page: number = 1,
      pageSize: number = 20
    ) => {
      setLoading(true);
      setError(null);

      try {
        const result = await productService.getProductsByCategory(
          masterCategoryId,
          subCategoryId,
          page,
          pageSize
        );

        if (result.status === 200 && result.data) {
          const response: GetProductsResponse = result.data;
          setProducts(response.products);
          setProductTotalCount(response.totalCount);
          setProductPage(response.page);
          setProductPageSize(response.pageSize);
          setProductTotalPages(response.totalPages);
        } else {
          setError(
            result.message || "Nie udało się pobrać produktów z kategorii"
          );
        }
      } catch (err) {
        setError("Błąd podczas pobierania produktów z kategorii");
        console.error("Error fetching products by category:", err);
      } finally {
        setLoading(false);
      }
    },
    []
  );

  const searchProducts = useCallback(async (query: string) => {
    setLoading(true);
    setError(null);

    try {
      const result = await productService.searchProducts(query);

      if (result.status === 200 && result.data) {
        const response: SearchProductsResponse = result.data;
        setProducts(response.products);
        setProductTotalCount(response.totalCount);
        setProductPage(response.page);
        setProductPageSize(response.pageSize);
        setProductTotalPages(response.totalPages);
      } else {
        setError(result.message || "Nie udało się wyszukać produktów");
      }
    } catch (err) {
      setError("Błąd podczas wyszukiwania produktów");
      console.error("Error searching products:", err);
    } finally {
      setLoading(false);
    }
  }, []);

  const loadCategories = useCallback(async () => {
    setCategoriesLoading(true);
    setCategoriesError(null);

    try {
      const masterResult = await productService.getMasterCategories();

      if (masterResult.status === 200 && masterResult.data) {
        setMasterCategories(masterResult.data || []);
      } else {
        setCategoriesError(
          masterResult.message || "Nie udało się załadować kategorii"
        );
      }
    } catch (err) {
      setCategoriesError("Błąd podczas ładowania kategorii");
      console.log(err);
    } finally {
      setCategoriesLoading(false);
    }
  }, []);

  return {
    // Products state
    products,
    currentProduct,
    totalProductCount,
    productPage,
    productPageSize,
    totalProductPages,
    loading,
    error,

    // Bestsellers state
    bestsellers,
    bestsellersLoading,
    bestsellersError,
    totalBestsellersCount,
    bestsellersPage,
    bestsellersPageSize,
    totalBestsellersPages,

    // New products state
    newProducts,
    newProductsLoading,
    newProductsError,
    totalNewProductsCount,
    newProductsPage,
    newProductsPageSize,
    totalNewProductsPages,

    // Categories state
    masterCategories,
    subCategories,
    articleTypes,
    baseColours,
    categoriesLoading,
    categoriesError,

    // Actions
    getProducts,
    getProductById,
    getBestsellers,
    getNewProducts,
    getProductsByCategory,
    searchProducts,
    loadCategories,
    clearError,
    clearBestsellersError,
    clearNewProductsError,
    clearCategoriesError,
    setSubCategories,
    setArticleTypes,
    setBaseColours,
  };
};
