import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'path'

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },
  base: './', // Importante para Electron
  build: {
    outDir: 'dist',
    assetsDir: 'assets',
  },
  server: {
    host: '0.0.0.0', // Escuchar en todas las interfaces para acceso desde dispositivos móviles
    port: 3000,
    strictPort: false,
    proxy: {
      '/api': {
        target: 'http://localhost:5002', // El proxy funciona desde el servidor de Vite, usa localhost
        changeOrigin: true,
        secure: false,
      }
    }
  }
})
