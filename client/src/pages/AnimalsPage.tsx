import { useEffect, useState } from 'react'
import { Link, useSearchParams } from 'react-router-dom'
import api from '../services/api'
import { Plus, Beef } from 'lucide-react'
import AddAnimalModal from '../components/AddAnimalModal'

interface Animal {
  id: number
  name: string
  breed: string
  birthDate: string
  sex: string
  currentWeight: number
  lastVaccineDate?: string
}

export default function AnimalsPage() {
  const [animals, setAnimals] = useState<Animal[]>([])
  const [loading, setLoading] = useState(true)
  const [showModal, setShowModal] = useState(false)
  const [searchParams] = useSearchParams()

  useEffect(() => {
    if (searchParams.get('new') === 'true') {
      setShowModal(true)
    }
    loadAnimals()
  }, [searchParams])

  const loadAnimals = async () => {
    try {
      const response = await api.get('/animals')
      setAnimals(response.data)
    } catch (error) {
      console.error('Error loading animals:', error)
    } finally {
      setLoading(false)
    }
  }

  const handleAnimalAdded = () => {
    setShowModal(false)
    loadAnimals()
  }

  if (loading) {
    return <div className="flex items-center justify-center min-h-[60vh]">Cargando...</div>
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h2 className="text-3xl font-bold text-gray-900 mb-2">Mis Animales</h2>
          <p className="text-gray-600">Gestiona tu ganado</p>
        </div>
        <button
          onClick={() => setShowModal(true)}
          className="bg-gradient-to-r from-green-600 to-green-700 text-white px-6 py-3 rounded-xl font-semibold hover:from-green-700 hover:to-green-800 transition-all flex items-center gap-2 shadow-lg"
        >
          <Plus className="h-5 w-5" />
          Agregar Animal
        </button>
      </div>

      {animals.length === 0 ? (
        <div className="bg-white rounded-2xl shadow-lg p-12 text-center">
          <Beef className="h-16 w-16 text-gray-400 mx-auto mb-4" />
          <h3 className="text-xl font-semibold text-gray-900 mb-2">No tienes animales registrados</h3>
          <p className="text-gray-600 mb-6">Comienza agregando tu primer animal</p>
          <button
            onClick={() => setShowModal(true)}
            className="bg-gradient-to-r from-green-600 to-green-700 text-white px-6 py-3 rounded-xl font-semibold hover:from-green-700 hover:to-green-800 transition-all"
          >
            Agregar Primer Animal
          </button>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {animals.map((animal) => (
            <Link
              key={animal.id}
              to={`/app/animals/${animal.id}`}
              className="bg-white rounded-2xl shadow-lg p-6 hover:shadow-xl transition-all border-2 border-transparent hover:border-green-500"
            >
              <div className="flex items-start justify-between mb-4">
                <div className="bg-green-100 p-3 rounded-xl">
                  <Beef className="h-6 w-6 text-green-600" />
                </div>
                <span className="text-xs font-semibold px-3 py-1 bg-gray-100 text-gray-700 rounded-full">
                  {animal.sex}
                </span>
              </div>
              <h3 className="text-xl font-bold text-gray-900 mb-2">{animal.name}</h3>
              <p className="text-gray-600 text-sm mb-1">Raza: {animal.breed}</p>
              <p className="text-gray-600 text-sm mb-4">Peso: {animal.currentWeight} kg</p>
              {animal.lastVaccineDate && (
                <p className="text-xs text-gray-500">
                  Última vacuna: {new Date(animal.lastVaccineDate).toLocaleDateString('es-ES')}
                </p>
              )}
            </Link>
          ))}
        </div>
      )}

      {showModal && <AddAnimalModal onClose={() => setShowModal(false)} onSuccess={handleAnimalAdded} />}
    </div>
  )
}
