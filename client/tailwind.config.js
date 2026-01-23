/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: '#2563eb',
          dark: '#1e40af',
        },
        secondary: {
          DEFAULT: '#10b981',
          dark: '#059669',
        },
        accent: {
          DEFAULT: '#f59e0b',
          dark: '#d97706',
        },
      },
    },
  },
  plugins: [],
}
