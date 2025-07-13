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

interface UseProductsReturn {
  // Products state
  products: ProductDto[];
  currentProduct: ProductDto | null;
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  loading: boolean;
  error: string | null;

  // Categories state
  masterCategories: GetMasterCategoriesResponse["masterCategories"];
  subCategories: GetSubCategoriesResponse["subCategories"];
  articleTypes: GetArticleTypesResponse["articleTypes"];
  baseColours: GetBaseColoursResponse["baseColours"];
  categoriesLoading: boolean;
  categoriesError: string | null;

  // Actions
  getProducts: (pageNumber?: number, pageSizeNumber?: number) => Promise<void>;
  getProductById: (productId: string) => Promise<void>;
  getBestsellers: () => Promise<void>;
  getNewProducts: () => Promise<void>;
  getProductsByCategory: (category: string) => Promise<void>;
  searchProducts: (query: string) => Promise<void>;
  loadCategories: () => Promise<void>;
  clearError: () => void;
  clearCategoriesError: () => void;
}

export const useProducts = (): UseProductsReturn => {
  // Products state
  const [products, setProducts] = useState<ProductDto[]>([]);
  const [currentProduct, setCurrentProduct] = useState<ProductDto | null>(null);
  const [totalCount, setTotalCount] = useState(0);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(20);
  const [totalPages, setTotalPages] = useState(0);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

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
          setTotalCount(response.totalCount);
          setPage(response.page);
          setPageSize(response.pageSize);
          setTotalPages(response.totalPages);
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

  const getBestsellers = useCallback(async () => {
    setLoading(true);
    setError(null);

    try {
      const result = await productService.getBestsellers();

      if (result.status === 200 && result.data) {
        const response: GetProductsResponse = result.data;
        setProducts(response.products);
        setTotalCount(response.totalCount);
        setPage(response.page);
        setPageSize(response.pageSize);
        setTotalPages(response.totalPages);
      } else {
        setError(result.message || "Nie udało się pobrać bestsellerów");
      }
    } catch (err) {
      setError("Błąd podczas pobierania bestsellerów");
      console.error("Error fetching bestsellers:", err);
    } finally {
      setLoading(false);
    }
  }, []);

  const getNewProducts = useCallback(async () => {
    setLoading(true);
    setError(null);

    try {
      const result = await productService.getNewProducts();

      if (result.status === 200 && result.data) {
        const response: GetProductsResponse = result.data;
        setProducts(response.products);
        setTotalCount(response.totalCount);
        setPage(response.page);
        setPageSize(response.pageSize);
        setTotalPages(response.totalPages);
      } else {
        setError(result.message || "Nie udało się pobrać nowych produktów");
      }
    } catch (err) {
      setError("Błąd podczas pobierania nowych produktów");
      console.error("Error fetching new products:", err);
    } finally {
      setLoading(false);
    }
  }, []);

  const getProductsByCategory = useCallback(async (category: string) => {
    setLoading(true);
    setError(null);

    try {
      const result = await productService.getProductsByCategory(category);

      if (result.status === 200 && result.data) {
        const response: GetProductsResponse = result.data;
        setProducts(response.products);
        setTotalCount(response.totalCount);
        setPage(response.page);
        setPageSize(response.pageSize);
        setTotalPages(response.totalPages);
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
  }, []);

  const searchProducts = useCallback(async (query: string) => {
    setLoading(true);
    setError(null);

    try {
      const result = await productService.searchProducts(query);

      if (result.status === 200 && result.data) {
        const response: SearchProductsResponse = result.data;
        setProducts(response.products);
        setTotalCount(response.totalCount);
        setPage(response.page);
        setPageSize(response.pageSize);
        setTotalPages(response.totalPages);
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
      // Load all categories in parallel
      const [masterResult, subResult, articleResult, coloursResult] =
        await Promise.all([
          productService.getMasterCategories(),
          productService.getSubCategories(),
          productService.getArticleTypes(),
          productService.getBaseColours(),
        ]);

      if (masterResult.status === 200 && masterResult.data) {
        setMasterCategories(masterResult.data.masterCategories);
      }

      if (subResult.status === 200 && subResult.data) {
        setSubCategories(subResult.data.subCategories);
      }

      if (articleResult.status === 200 && articleResult.data) {
        setArticleTypes(articleResult.data.articleTypes);
      }

      if (coloursResult.status === 200 && coloursResult.data) {
        setBaseColours(coloursResult.data.baseColours);
      }

      // Check for errors
      const errors = [
        masterResult.message,
        subResult.message,
        articleResult.message,
        coloursResult.message,
      ].filter(Boolean);

      if (errors.length > 0) {
        setCategoriesError(
          `Błędy podczas ładowania kategorii: ${errors.join(", ")}`
        );
      }
    } catch (err) {
      setCategoriesError("Błąd podczas ładowania kategorii");
      console.error("Error loading categories:", err);
    } finally {
      setCategoriesLoading(false);
    }
  }, []);

  return {
    // Products state
    products,
    currentProduct,
    totalCount,
    page,
    pageSize,
    totalPages,
    loading,
    error,

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
    clearCategoriesError,
  };
};
