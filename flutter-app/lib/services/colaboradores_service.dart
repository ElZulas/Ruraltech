import 'dart:convert';
import 'package:http/http.dart' as http;
import 'api_service.dart';

class ColaboradoresService {
  static Future<List<dynamic>> getColaboradoresByUPP(String uppId) async {
    final response = await http.get(
      Uri.parse('${ApiService.baseUrl}/colaboradores/upp/$uppId'),
      headers: await ApiService.getHeaders(),
    );

    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    } else {
      throw Exception('Error al obtener colaboradores: ${response.body}');
    }
  }

  static Future<Map<String, dynamic>> createColaborador(Map<String, dynamic> colaboradorData) async {
    final response = await http.post(
      Uri.parse('${ApiService.baseUrl}/colaboradores'),
      headers: await ApiService.getHeaders(),
      body: jsonEncode(colaboradorData),
    );

    if (response.statusCode == 200 || response.statusCode == 201) {
      return jsonDecode(response.body);
    } else {
      final errorData = jsonDecode(response.body);
      throw Exception(errorData['message'] ?? 'Error al crear colaborador');
    }
  }

  static Future<Map<String, dynamic>> updateColaborador(String id, Map<String, dynamic> colaboradorData) async {
    final response = await http.put(
      Uri.parse('${ApiService.baseUrl}/colaboradores/$id'),
      headers: await ApiService.getHeaders(),
      body: jsonEncode(colaboradorData),
    );

    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    } else {
      final errorData = jsonDecode(response.body);
      throw Exception(errorData['message'] ?? 'Error al actualizar colaborador');
    }
  }

  static Future<void> deleteColaborador(String id) async {
    final response = await http.delete(
      Uri.parse('${ApiService.baseUrl}/colaboradores/$id'),
      headers: await ApiService.getHeaders(),
    );

    if (response.statusCode != 204 && response.statusCode != 200) {
      final errorData = jsonDecode(response.body);
      throw Exception(errorData['message'] ?? 'Error al eliminar colaborador');
    }
  }

  static Future<Map<String, dynamic>> updateEstatus(String id, String estatus) async {
    final response = await http.patch(
      Uri.parse('${ApiService.baseUrl}/colaboradores/$id/estatus'),
      headers: await ApiService.getHeaders(),
      body: jsonEncode(estatus),
    );

    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    } else {
      final errorData = jsonDecode(response.body);
      throw Exception(errorData['message'] ?? 'Error al actualizar estatus');
    }
  }
}
