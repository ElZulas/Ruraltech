import { useEffect, useState } from 'react'
import api from '../services/api'
import { ShoppingBag, MapPin, Phone, MessageCircle, Star } from 'lucide-react'

interface Product {
  id: number
  name: string
  description: string
  price: number
  category: string
  imageUrl?: string
  sellerName: string
  location?: string
  phone?: string
  whatsApp?: string
  rating?: number
  reviewCount: number
  isFeatured: boolean
}

export default function MarketplacePage() {
  const [products, setProducts] = useState<Product[]>([])
  const [categories, setCategories] = useState<string[]>([])
  const [selectedCategory, setSelectedCategory] = useState<string>('')
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    loadCategories()
    loadProducts()
  }, [])

  useEffect(() => {
    loadProducts()
  }, [selectedCategory])

  const loadCategories = async () => {
    try {
      const response = await api.get('/products/categories')
      setCategories(response.data)
    } catch (error) {
      console.error('Error loading categories:', error)
    }
  }

  const loadProducts = async () => {
    try {
      const params: any = {}
      if (selectedCategory) {
        params.category = selectedCategory
      }
      const response = await api.get('/products', { params })
      setProducts(response.data)
    } catch (error) {
      console.error('Error loading products:', error)
    } finally {
      setLoading(false)
    }
  }

  const formatPrice = (price: number) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0,
    }).format(price)
  }

  const handleContact = (product: Product) => {
    if (product.whatsApp) {
      const message = `Hola, estoy interesado en ${product.name} publicado en RuralTech.`
      const whatsappUrl = `https://wa.me/${product.whatsApp.replace(/[^0-9]/g, '')}?text=${encodeURIComponent(message)}`
      window.open(whatsappUrl, '_blank')
    } else if (product.phone) {
      window.open(`tel:${product.phone}`, '_blank')
    }
  }

  if (loading) {
    return <div className="flex items-center justify-center min-h-[60vh]">Cargando...</div>
  }

  return (
    <div className="space-y-6">
      <div>
        <h2 className="text-3xl font-bold text-gray-900 mb-2">Marketplace</h2>
        <p className="text-gray-600">Todo lo que necesitas para tu ganado</p>
      </div>

      {/* Categories */}
      {categories.length > 0 && (
        <div className="flex flex-wrap gap-2">
          <button
            onClick={() => setSelectedCategory('')}
            className={`px-4 py-2 rounded-xl font-medium transition-colors ${
              selectedCategory === ''
                ? 'bg-gradient-to-r from-blue-600 to-green-600 text-white'
                : 'bg-white text-gray-700 hover:bg-gray-100'
            }`}
          >
            Todas
          </button>
          {categories.map((category) => (
            <button
              key={category}
              onClick={() => setSelectedCategory(category)}
              className={`px-4 py-2 rounded-xl font-medium transition-colors ${
                selectedCategory === category
                  ? 'bg-gradient-to-r from-blue-600 to-green-600 text-white'
                  : 'bg-white text-gray-700 hover:bg-gray-100'
              }`}
            >
              {category}
            </button>
          ))}
        </div>
      )}

      {/* Products */}
      {products.length === 0 ? (
        <div className="bg-white rounded-2xl shadow-lg p-12 text-center">
          <ShoppingBag className="h-16 w-16 text-gray-400 mx-auto mb-4" />
          <h3 className="text-xl font-semibold text-gray-900 mb-2">No hay productos disponibles</h3>
          <p className="text-gray-600">Intenta con otra categoría</p>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {products.map((product) => (
            <div
              key={product.id}
              className="bg-white rounded-2xl shadow-lg overflow-hidden hover:shadow-xl transition-all border-2 border-transparent hover:border-orange-500"
            >
              {product.imageUrl ? (
                <img src={product.imageUrl} alt={product.name} className="w-full h-48 object-cover" />
              ) : (
                <div className="w-full h-48 bg-gradient-to-br from-orange-100 to-orange-200 flex items-center justify-center">
                  <ShoppingBag className="h-16 w-16 text-orange-400" />
                </div>
              )}
              <div className="p-6">
                <div className="flex items-start justify-between mb-2">
                  <h3 className="text-xl font-bold text-gray-900">{product.name}</h3>
                  {product.isFeatured && (
                    <span className="text-xs font-semibold px-2 py-1 bg-yellow-100 text-yellow-700 rounded-full">
                      Destacado
                    </span>
                  )}
                </div>
                <p className="text-gray-600 text-sm mb-4 line-clamp-2">{product.description}</p>
                <div className="flex items-center justify-between mb-4">
                  <p className="text-2xl font-bold text-green-600">{formatPrice(product.price)}</p>
                  {product.rating && (
                    <div className="flex items-center gap-1">
                      <Star className="h-4 w-4 fill-yellow-400 text-yellow-400" />
                      <span className="text-sm font-medium">{product.rating}</span>
                      <span className="text-xs text-gray-500">({product.reviewCount})</span>
                    </div>
                  )}
                </div>
                <div className="space-y-2 mb-4">
                  <div className="flex items-center gap-2 text-sm text-gray-600">
                    <MapPin className="h-4 w-4" />
                    <span>{product.location || 'No especificado'}</span>
                  </div>
                  <div className="flex items-center gap-2 text-sm text-gray-600">
                    <span>{product.sellerName}</span>
                  </div>
                </div>
                <button
                  onClick={() => handleContact(product)}
                  className="w-full bg-gradient-to-r from-green-600 to-green-700 text-white py-3 rounded-xl font-semibold hover:from-green-700 hover:to-green-800 transition-all flex items-center justify-center gap-2"
                >
                  {product.whatsApp ? (
                    <>
                      <MessageCircle className="h-5 w-5" />
                      Contactar por WhatsApp
                    </>
                  ) : product.phone ? (
                    <>
                      <Phone className="h-5 w-5" />
                      Llamar
                    </>
                  ) : (
                    'Ver Detalles'
                  )}
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}
