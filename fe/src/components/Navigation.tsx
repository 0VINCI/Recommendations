import React from 'react';
import { Link, useLocation } from 'react-router-dom';

const categories = [
  { id: 'wszystkie', name: 'Wszystkie', path: '/' },
  { id: 'koszule', name: 'Koszule', path: '/category/koszule' },
  { id: 'spodnie', name: 'Spodnie', path: '/category/spodnie' },
  { id: 'sukienki', name: 'Sukienki', path: '/category/sukienki' },
  { id: 'bluzy', name: 'Bluzy', path: '/category/bluzy' },
  { id: 'marynarki', name: 'Marynarki', path: '/category/marynarki' },
  { id: 'koszulki', name: 'Koszulki', path: '/category/koszulki' },
  { id: 'spódnice', name: 'Spódnice', path: '/category/spódnice' },
  { id: 'swetry', name: 'Swetry', path: '/category/swetry' },
];

export function Navigation() {
  const location = useLocation();

  return (
    <nav className="bg-gray-50 dark:bg-gray-800 border-b border-gray-200 dark:border-gray-700">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex space-x-8 overflow-x-auto py-4">
          {categories.map((category) => {
            const isActive = location.pathname === category.path;
            return (
              <Link
                key={category.id}
                to={category.path}
                className={`whitespace-nowrap px-3 py-2 text-sm font-medium rounded-md transition-colors ${
                  isActive
                    ? 'bg-primary-100 dark:bg-primary-900 text-primary-700 dark:text-primary-300'
                    : 'text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white hover:bg-gray-100 dark:hover:bg-gray-700'
                }`}
              >
                {category.name}
              </Link>
            );
          })}
        </div>
      </div>
    </nav>
  );
}