export enum EmbeddingSource {
  New = "new",
  Old = "old",
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
    description: "Nowe embeddingi (domyślne)",
  },
  {
    value: EmbeddingSource.Old,
    label: "Sentence BERT",
    description: "Stare embeddingi",
  },
];

