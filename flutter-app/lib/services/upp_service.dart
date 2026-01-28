import 'dart:convert';
import 'package:http/http.dart' as http;
import 'api_service.dart';

class UPPService {
  static Future<List<dynamic>> getUPPs() async {
    final response = await http.get(
      Uri.parse('${ApiService.baseUrl}/upps'),
      headers: await ApiService.getHeaders(),
    );

    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    } else {
      throw Exception('Error al obtener UPPs: ${response.body}');
    }
  }

  static Future<Map<String, dynamic>> createUPP(Map<String, dynamic> uppData) async {
    final response = await http.post(
      Uri.parse('${ApiService.baseUrl}/upps'),
      headers: await ApiService.getHeaders(),
      body: jsonEncode(uppData),
    );

    if (response.statusCode == 200 || response.statusCode == 201) {
      return jsonDecode(response.body);
    } else {
      final errorData = jsonDecode(response.body);
      throw Exception(errorData['message'] ?? 'Error al crear UPP');
    }
  }

  static Future<Map<String, dynamic>> updateUPP(String id, Map<String, dynamic> uppData) async {
    final response = await http.put(
      Uri.parse('${ApiService.baseUrl}/upps/$id'),
      headers: await ApiService.getHeaders(),
      body: jsonEncode(uppData),
    );

    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    } else {
      final errorData = jsonDecode(response.body);
      throw Exception(errorData['message'] ?? 'Error al actualizar UPP');
    }
  }

  static Future<void> deleteUPP(String id) async {
    final response = await http.delete(
      Uri.parse('${ApiService.baseUrl}/upps/$id'),
      headers: await ApiService.getHeaders(),
    );

    if (response.statusCode != 204 && response.statusCode != 200) {
      final errorData = jsonDecode(response.body);
      throw Exception(errorData['message'] ?? 'Error al eliminar UPP');
    }
  }
}
