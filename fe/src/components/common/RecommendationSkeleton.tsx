export function RecommendationSkeleton() {
  return (
    <div className="py-8">
      <div className="mb-6">
        <div className="h-8 bg-gray-200 dark:bg-gray-700 rounded-lg w-64 mb-2 animate-pulse"></div>
        <div className="h-4 bg-gray-200 dark:bg-gray-700 rounded w-48 mb-1 animate-pulse"></div>
        <div className="h-3 bg-gray-200 dark:bg-gray-700 rounded w-32 animate-pulse"></div>
      </div>

      {/* Carousel Skeleton */}
      <div className="relative">
        {/* Left Arrow Skeleton */}
        <div className="absolute left-0 top-1/2 -translate-y-1/2 z-10 bg-white dark:bg-gray-800 rounded-full p-2 shadow-lg border border-gray-200 dark:border-gray-700">
          <div className="w-5 h-5 bg-gray-200 dark:bg-gray-700 rounded animate-pulse"></div>
        </div>

        {/* Right Arrow Skeleton */}
        <div className="absolute right-0 top-1/2 -translate-y-1/2 z-10 bg-white dark:bg-gray-800 rounded-full p-2 shadow-lg border border-gray-200 dark:border-gray-700">
          <div className="w-5 h-5 bg-gray-200 dark:bg-gray-700 rounded animate-pulse"></div>
        </div>

        {/* Scrollable Products Container Skeleton */}
        <div className="flex gap-6 overflow-x-auto scrollbar-hide pb-4">
          {Array.from({ length: 4 }).map((_, index) => (
            <div key={index} className="flex-shrink-0 w-80">
              <div className="bg-white dark:bg-gray-800 rounded-2xl shadow-soft border border-gray-100 dark:border-gray-700 overflow-hidden">
                {/* Image skeleton */}
                <div className="aspect-square bg-gray-200 dark:bg-gray-700 animate-pulse"></div>

                {/* Content skeleton */}
                <div className="p-4">
                  <div className="h-4 bg-gray-200 dark:bg-gray-700 rounded w-3/4 mb-2 animate-pulse"></div>
                  <div className="h-3 bg-gray-200 dark:bg-gray-700 rounded w-1/2 mb-3 animate-pulse"></div>

                  {/* Price skeleton */}
                  <div className="flex items-center justify-between mb-3">
                    <div className="h-5 bg-gray-200 dark:bg-gray-700 rounded w-16 animate-pulse"></div>
                    <div className="h-4 bg-gray-200 dark:bg-gray-700 rounded w-12 animate-pulse"></div>
                  </div>

                  {/* Rating skeleton */}
                  <div className="flex items-center space-x-1 mb-3">
                    <div className="h-4 bg-gray-200 dark:bg-gray-700 rounded w-16 animate-pulse"></div>
                  </div>

                  {/* Button skeleton */}
                  <div className="h-10 bg-gray-200 dark:bg-gray-700 rounded-lg animate-pulse"></div>
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
