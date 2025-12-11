import { useApp } from "../../context/useApp";
import { EmbeddingSource } from "../../types/recommendation/EmbeddingSource";

export function EmbeddingSourceSelector() {
  const { state, dispatch } = useApp();
  const isNew = state.selectedEmbeddingSource === EmbeddingSource.New;

  const toggle = () => {
    dispatch({
      type: "SET_EMBEDDING_SOURCE",
      payload: isNew ? EmbeddingSource.Old : EmbeddingSource.New,
    });
  };

  return (
    <button
      onClick={toggle}
      className="flex items-center space-x-2 px-3 py-2 text-sm text-gray-700 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white transition-colors rounded-md hover:bg-gray-100 dark:hover:bg-gray-700"
      title="Przełącz źródło embeddingów"
    >
      <div className="flex items-center space-x-2">
        <span className={`font-medium ${!isNew ? "text-brand-600" : ""}`}>
          SBERT
        </span>
        <div
          className={`w-12 h-6 flex items-center rounded-full p-1 transition-colors ${
            isNew
              ? "bg-brand-500/80"
              : "bg-gray-300 dark:bg-gray-600"
          }`}
        >
          <div
            className={`bg-white w-4 h-4 rounded-full shadow transform transition-transform ${
              isNew ? "translate-x-6" : ""
            }`}
          ></div>
        </div>
        <span className={`font-medium ${isNew ? "text-brand-600" : ""}`}>
          Qwen3
        </span>
      </div>
    </button>
  );
}

