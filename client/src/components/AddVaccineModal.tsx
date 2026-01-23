import { useState } from 'react'
import api from '../services/api'
import { X } from 'lucide-react'

interface AddVaccineModalProps {
  animalId: number
  onClose: () => void
  onSuccess: () => void
}

export default function AddVaccineModal({ animalId, onClose, onSuccess }: AddVaccineModalProps) {
  const [formData, setFormData] = useState({
    name: '',
    dateApplied: new Date().toISOString().split('T')[0],
    nextDueDate: '',
    notes: '',
  })
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value })
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError('')
    setLoading(true)

    try {
      await api.post(`/animals/${animalId}/vaccines`, {
        ...formData,
        dateApplied: new Date(formData.dateApplied).toISOString(),
        nextDueDate: new Date(formData.nextDueDate).toISOString(),
        notes: formData.notes || undefined,
      })
      onSuccess()
    } catch (err: any) {
      setError(err.response?.data?.message || 'Error al agregar vacuna')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-2xl max-w-md w-full p-6 relative">
        <button onClick={onClose} className="absolute top-4 right-4 text-gray-400 hover:text-gray-600">
          <X className="h-5 w-5" />
        </button>

        <h2 className="text-2xl font-bold text-gray-900 mb-6">Agregar Vacuna</h2>

        {error && (
          <div className="mb-4 p-3 bg-red-50 border border-red-200 text-red-700 rounded-lg text-sm">
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label htmlFor="name" className="block text-sm font-medium text-gray-700 mb-1">
              Nombre de la Vacuna
            </label>
            <input
              id="name"
              name="name"
              type="text"
              value={formData.name}
              onChange={handleChange}
              required
              className="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="Ej: Brucelosis"
            />
          </div>

          <div>
            <label htmlFor="dateApplied" className="block text-sm font-medium text-gray-700 mb-1">
              Fecha de Aplicación
            </label>
            <input
              id="dateApplied"
              name="dateApplied"
              type="date"
              value={formData.dateApplied}
              onChange={handleChange}
              required
              className="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
          </div>

          <div>
            <label htmlFor="nextDueDate" className="block text-sm font-medium text-gray-700 mb-1">
              Próxima Fecha
            </label>
            <input
              id="nextDueDate"
              name="nextDueDate"
              type="date"
              value={formData.nextDueDate}
              onChange={handleChange}
              required
              className="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
          </div>

          <div>
            <label htmlFor="notes" className="block text-sm font-medium text-gray-700 mb-1">
              Notas (Opcional)
            </label>
            <textarea
              id="notes"
              name="notes"
              value={formData.notes}
              onChange={handleChange}
              className="w-full px-4 py-3 border border-gray-300 rounded-xl focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              rows={3}
            />
          </div>

          <div className="flex gap-3 pt-4">
            <button
              type="button"
              onClick={onClose}
              className="flex-1 px-4 py-3 border border-gray-300 text-gray-700 rounded-xl font-semibold hover:bg-gray-50 transition-colors"
            >
              Cancelar
            </button>
            <button
              type="submit"
              disabled={loading}
              className="flex-1 bg-gradient-to-r from-blue-600 to-blue-700 text-white py-3 rounded-xl font-semibold hover:from-blue-700 hover:to-blue-800 transition-all disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {loading ? 'Guardando...' : 'Guardar'}
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}
