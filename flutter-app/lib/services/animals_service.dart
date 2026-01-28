import 'dart:convert';
import 'package:http/http.dart' as http;
import 'api_service.dart';

class AnimalsService {
  static Future<List<dynamic>> getAnimals() async {
    final response = await http.get(
      Uri.parse('${ApiService.baseUrl}/animals'),
      headers: await ApiService.getHeaders(),
    );

    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    } else {
      throw Exception('Error al obtener animales: ${response.body}');
    }
  }

  static Future<Map<String, dynamic>> getAnimal(String id) async {
    final response = await http.get(
      Uri.parse('${ApiService.baseUrl}/animals/$id'),
      headers: await ApiService.getHeaders(),
    );

    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    } else {
      throw Exception('Error al obtener animal: ${response.body}');
    }
  }

  static Future<Map<String, dynamic>> createAnimal(Map<String, dynamic> animalData) async {
    final response = await http.post(
      Uri.parse('${ApiService.baseUrl}/animals'),
      headers: await ApiService.getHeaders(),
      body: jsonEncode(animalData),
    );

    if (response.statusCode == 200 || response.statusCode == 201) {
      return jsonDecode(response.body);
    } else {
      final errorData = jsonDecode(response.body);
      throw Exception(errorData['message'] ?? 'Error al crear animal');
    }
  }

  static Future<Map<String, dynamic>> updateAnimal(String id, Map<String, dynamic> animalData) async {
    final response = await http.put(
      Uri.parse('${ApiService.baseUrl}/animals/$id'),
      headers: await ApiService.getHeaders(),
      body: jsonEncode(animalData),
    );

    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    } else {
      final errorData = jsonDecode(response.body);
      throw Exception(errorData['message'] ?? 'Error al actualizar animal');
    }
  }

  static Future<void> deleteAnimal(String id) async {
    final response = await http.delete(
      Uri.parse('${ApiService.baseUrl}/animals/$id'),
      headers: await ApiService.getHeaders(),
    );

    if (response.statusCode != 204 && response.statusCode != 200) {
      final errorData = jsonDecode(response.body);
      throw Exception(errorData['message'] ?? 'Error al eliminar animal');
    }
  }

  static Future<Map<String, dynamic>> addWeight(String id, Map<String, dynamic> weightData) async {
    final response = await http.post(
      Uri.parse('${ApiService.baseUrl}/animals/$id/weight'),
      headers: await ApiService.getHeaders(),
      body: jsonEncode(weightData),
    );

    if (response.statusCode == 200 || response.statusCode == 201) {
      return jsonDecode(response.body);
    } else {
      final errorData = jsonDecode(response.body);
      throw Exception(errorData['message'] ?? 'Error al agregar peso');
    }
  }

  static Future<Map<String, dynamic>> addVaccine(String id, Map<String, dynamic> vaccineData) async {
    final response = await http.post(
      Uri.parse('${ApiService.baseUrl}/animals/$id/vaccines'),
      headers: await ApiService.getHeaders(),
      body: jsonEncode(vaccineData),
    );

    if (response.statusCode == 200 || response.statusCode == 201) {
      return jsonDecode(response.body);
    } else {
      final errorData = jsonDecode(response.body);
      throw Exception(errorData['message'] ?? 'Error al agregar vacuna');
    }
  }

  static Future<Map<String, dynamic>> addTreatment(String id, Map<String, dynamic> treatmentData) async {
    final response = await http.post(
      Uri.parse('${ApiService.baseUrl}/animals/$id/treatments'),
      headers: await ApiService.getHeaders(),
      body: jsonEncode(treatmentData),
    );

    if (response.statusCode == 200 || response.statusCode == 201) {
      return jsonDecode(response.body);
    } else {
      final errorData = jsonDecode(response.body);
      throw Exception(errorData['message'] ?? 'Error al agregar tratamiento');
    }
  }
}
