export enum EmbeddingSource {
  New = "new",
}

export interface EmbeddingSourceOption {
  value: EmbeddingSource;
  label: string;
  description: string;
}

export const EMBEDDING_SOURCES: EmbeddingSourceOption[] = [
  {
    value: EmbeddingSource.New,
    label: "Qwen3-Embedding-4B",
    description: "Embeddingi (domyślne)",
  },
];

