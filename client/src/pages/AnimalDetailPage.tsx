import { useEffect, useState } from 'react'
import { useParams, Link } from 'react-router-dom'
import api from '../services/api'
import { ArrowLeft, Scale, Syringe, Heart, Plus } from 'lucide-react'
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts'
import AddWeightModal from '../components/AddWeightModal'
import AddVaccineModal from '../components/AddVaccineModal'
import AddTreatmentModal from '../components/AddTreatmentModal'

interface AnimalDetail {
  id: number
  name: string
  breed: string
  birthDate: string
  sex: string
  currentWeight: number
  lastVaccineDate?: string
  weightHistory: Array<{ id: number; weight: number; date: string }>
  vaccines: Array<{ id: number; name: string; dateApplied: string; nextDueDate: string }>
  treatments: Array<{ id: number; condition: string; treatmentDescription: string; date: string }>
}

export default function AnimalDetailPage() {
  const { id } = useParams()
  const [animal, setAnimal] = useState<AnimalDetail | null>(null)
  const [loading, setLoading] = useState(true)
  const [showWeightModal, setShowWeightModal] = useState(false)
  const [showVaccineModal, setShowVaccineModal] = useState(false)
  const [showTreatmentModal, setShowTreatmentModal] = useState(false)

  useEffect(() => {
    loadAnimal()
  }, [id])

  const loadAnimal = async () => {
    try {
      const response = await api.get(`/animals/${id}`)
      setAnimal(response.data)
    } catch (error) {
      console.error('Error loading animal:', error)
    } finally {
      setLoading(false)
    }
  }

  const handleDataAdded = () => {
    loadAnimal()
  }

  if (loading) {
    return <div className="flex items-center justify-center min-h-[60vh]">Cargando...</div>
  }

  if (!animal) {
    return <div className="text-center">Animal no encontrado</div>
  }

  const chartData = animal.weightHistory
    .sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime())
    .map((w) => ({
      date: new Date(w.date).toLocaleDateString('es-ES', { month: 'short', day: 'numeric' }),
      peso: w.weight,
    }))

  return (
    <div className="space-y-6">
      <Link
        to="/app/animals"
        className="inline-flex items-center gap-2 text-gray-600 hover:text-gray-900 transition-colors"
      >
        <ArrowLeft className="h-5 w-5" />
        Volver a animales
      </Link>

      <div className="bg-white rounded-2xl shadow-lg p-6">
        <div className="flex items-start justify-between mb-6">
          <div>
            <h2 className="text-3xl font-bold text-gray-900 mb-2">{animal.name}</h2>
            <p className="text-gray-600">{animal.breed} • {animal.sex}</p>
          </div>
          <div className="text-right">
            <p className="text-sm text-gray-600">Peso Actual</p>
            <p className="text-2xl font-bold text-green-600">{animal.currentWeight} kg</p>
          </div>
        </div>

        <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
          <div className="bg-blue-50 p-4 rounded-xl">
            <p className="text-sm text-gray-600 mb-1">Fecha de Nacimiento</p>
            <p className="font-semibold text-gray-900">
              {new Date(animal.birthDate).toLocaleDateString('es-ES')}
            </p>
          </div>
          <div className="bg-green-50 p-4 rounded-xl">
            <p className="text-sm text-gray-600 mb-1">Edad</p>
            <p className="font-semibold text-gray-900">
              {Math.floor((new Date().getTime() - new Date(animal.birthDate).getTime()) / (1000 * 60 * 60 * 24 * 30))} meses
            </p>
          </div>
          <div className="bg-orange-50 p-4 rounded-xl">
            <p className="text-sm text-gray-600 mb-1">Vacunas</p>
            <p className="font-semibold text-gray-900">{animal.vaccines.length}</p>
          </div>
          <div className="bg-red-50 p-4 rounded-xl">
            <p className="text-sm text-gray-600 mb-1">Tratamientos</p>
            <p className="font-semibold text-gray-900">{animal.treatments.length}</p>
          </div>
        </div>
      </div>

      {/* Weight Chart */}
      {chartData.length > 0 && (
        <div className="bg-white rounded-2xl shadow-lg p-6">
          <div className="flex items-center justify-between mb-4">
            <h3 className="text-xl font-bold text-gray-900 flex items-center gap-2">
              <Scale className="h-5 w-5 text-green-600" />
              Historial de Peso
            </h3>
            <button
              onClick={() => setShowWeightModal(true)}
              className="text-sm bg-green-100 text-green-700 px-4 py-2 rounded-lg hover:bg-green-200 transition-colors flex items-center gap-2"
            >
              <Plus className="h-4 w-4" />
              Agregar
            </button>
          </div>
          <ResponsiveContainer width="100%" height={300}>
            <LineChart data={chartData}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="date" />
              <YAxis />
              <Tooltip />
              <Line type="monotone" dataKey="peso" stroke="#10b981" strokeWidth={2} />
            </LineChart>
          </ResponsiveContainer>
        </div>
      )}

      {/* Vaccines */}
      <div className="bg-white rounded-2xl shadow-lg p-6">
        <div className="flex items-center justify-between mb-4">
          <h3 className="text-xl font-bold text-gray-900 flex items-center gap-2">
            <Syringe className="h-5 w-5 text-blue-600" />
            Vacunas
          </h3>
          <button
            onClick={() => setShowVaccineModal(true)}
            className="text-sm bg-blue-100 text-blue-700 px-4 py-2 rounded-lg hover:bg-blue-200 transition-colors flex items-center gap-2"
          >
            <Plus className="h-4 w-4" />
            Agregar
          </button>
        </div>
        {animal.vaccines.length === 0 ? (
          <p className="text-gray-500 text-center py-8">No hay vacunas registradas</p>
        ) : (
          <div className="space-y-3">
            {animal.vaccines.map((vaccine) => (
              <div key={vaccine.id} className="border border-gray-200 rounded-xl p-4">
                <div className="flex items-start justify-between">
                  <div>
                    <h4 className="font-semibold text-gray-900">{vaccine.name}</h4>
                    <p className="text-sm text-gray-600">
                      Aplicada: {new Date(vaccine.dateApplied).toLocaleDateString('es-ES')}
                    </p>
                    <p className="text-sm text-gray-600">
                      Próxima: {new Date(vaccine.nextDueDate).toLocaleDateString('es-ES')}
                    </p>
                  </div>
                  {new Date(vaccine.nextDueDate) <= new Date(Date.now() + 30 * 24 * 60 * 60 * 1000) && (
                    <span className="text-xs font-semibold px-2 py-1 bg-red-100 text-red-700 rounded-full">
                      Próxima
                    </span>
                  )}
                </div>
              </div>
            ))}
          </div>
        )}
      </div>

      {/* Treatments */}
      <div className="bg-white rounded-2xl shadow-lg p-6">
        <div className="flex items-center justify-between mb-4">
          <h3 className="text-xl font-bold text-gray-900 flex items-center gap-2">
            <Heart className="h-5 w-5 text-red-600" />
            Tratamientos
          </h3>
          <button
            onClick={() => setShowTreatmentModal(true)}
            className="text-sm bg-red-100 text-red-700 px-4 py-2 rounded-lg hover:bg-red-200 transition-colors flex items-center gap-2"
          >
            <Plus className="h-4 w-4" />
            Agregar
          </button>
        </div>
        {animal.treatments.length === 0 ? (
          <p className="text-gray-500 text-center py-8">No hay tratamientos registrados</p>
        ) : (
          <div className="space-y-3">
            {animal.treatments.map((treatment) => (
              <div key={treatment.id} className="border border-gray-200 rounded-xl p-4">
                <h4 className="font-semibold text-gray-900 mb-1">{treatment.condition}</h4>
                <p className="text-sm text-gray-600 mb-2">{treatment.treatmentDescription}</p>
                <p className="text-xs text-gray-500">
                  {new Date(treatment.date).toLocaleDateString('es-ES')}
                </p>
              </div>
            ))}
          </div>
        )}
      </div>

      {showWeightModal && (
        <AddWeightModal
          animalId={animal.id}
          onClose={() => setShowWeightModal(false)}
          onSuccess={handleDataAdded}
        />
      )}
      {showVaccineModal && (
        <AddVaccineModal
          animalId={animal.id}
          onClose={() => setShowVaccineModal(false)}
          onSuccess={handleDataAdded}
        />
      )}
      {showTreatmentModal && (
        <AddTreatmentModal
          animalId={animal.id}
          onClose={() => setShowTreatmentModal(false)}
          onSuccess={handleDataAdded}
        />
      )}
    </div>
  )
}
