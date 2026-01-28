import 'dart:convert';
import 'package:http/http.dart' as http;
import 'api_service.dart';

class BovinosService {
  static Future<List<dynamic>> getBovinosByUPP(String uppId) async {
    final response = await http.get(
      Uri.parse('${ApiService.baseUrl}/bovinos/upp/$uppId'),
      headers: await ApiService.getHeaders(),
    );

    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    } else {
      throw Exception('Error al obtener bovinos: ${response.body}');
    }
  }

  static Future<Map<String, dynamic>> createBovino(Map<String, dynamic> bovinoData) async {
    final response = await http.post(
      Uri.parse('${ApiService.baseUrl}/bovinos'),
      headers: await ApiService.getHeaders(),
      body: jsonEncode(bovinoData),
    );

    if (response.statusCode == 200 || response.statusCode == 201) {
      return jsonDecode(response.body);
    } else {
      final errorData = jsonDecode(response.body);
      throw Exception(errorData['message'] ?? 'Error al crear bovino');
    }
  }

  static Future<Map<String, dynamic>> updateBovino(String id, Map<String, dynamic> bovinoData) async {
    final response = await http.put(
      Uri.parse('${ApiService.baseUrl}/bovinos/$id'),
      headers: await ApiService.getHeaders(),
      body: jsonEncode(bovinoData),
    );

    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    } else {
      final errorData = jsonDecode(response.body);
      throw Exception(errorData['message'] ?? 'Error al actualizar bovino');
    }
  }

  static Future<void> deleteBovino(String id) async {
    final response = await http.delete(
      Uri.parse('${ApiService.baseUrl}/bovinos/$id'),
      headers: await ApiService.getHeaders(),
    );

    if (response.statusCode != 204 && response.statusCode != 200) {
      final errorData = jsonDecode(response.body);
      throw Exception(errorData['message'] ?? 'Error al eliminar bovino');
    }
  }
}
