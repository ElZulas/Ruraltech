import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'

// Aplicar imagen de fondo al body usando la ruta correcta de Vite
const baseUrl = import.meta.env.BASE_URL || '/'
const backgroundImageUrl = `${baseUrl}images/cesped_front.jpg`

const applyBackground = () => {
  if (document.body) {
    document.body.style.backgroundImage = `url(${backgroundImageUrl})`
    document.body.style.backgroundSize = 'cover'
    document.body.style.backgroundPosition = 'center'
    document.body.style.backgroundRepeat = 'no-repeat'
    document.body.style.backgroundAttachment = 'fixed'
    document.body.style.minHeight = '100vh'
  }
}

// Aplicar cuando el DOM esté listo
if (document.readyState === 'loading') {
  document.addEventListener('DOMContentLoaded', applyBackground)
} else {
  applyBackground()
}

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
)
