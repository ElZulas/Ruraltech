import 'dart:convert';
import 'dart:io';
import 'package:http/http.dart' as http;
import 'package:shared_preferences/shared_preferences.dart';

import '../config/api_config.dart';

class ApiService {
  // La URL base se configura en api_config.dart
  // Se obtiene dinámicamente según el entorno (development/production)
  static String get baseUrl => ApiConfig.baseUrl;
  
  // Timeout para las peticiones (30 segundos)
  static const Duration timeout = Duration(seconds: 30);
  
  static Future<String?> getToken() async {
    final prefs = await SharedPreferences.getInstance();
    return prefs.getString('token');
  }

  static Future<void> saveToken(String token) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setString('token', token);
  }

  static Future<void> clearToken() async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.remove('token');
  }

  static Future<Map<String, String>> getHeaders({bool includeAuth = true}) async {
    final headers = <String, String>{
      'Content-Type': 'application/json',
    };

    if (includeAuth) {
      final token = await getToken();
      if (token != null) {
        headers['Authorization'] = 'Bearer $token';
      }
    }

    return headers;
  }

  static Future<Map<String, dynamic>> login(String email, String password) async {
    try {
      final response = await http
          .post(
            Uri.parse('$baseUrl/auth/login'),
            headers: await getHeaders(includeAuth: false),
            body: jsonEncode({'email': email, 'password': password}),
          )
          .timeout(timeout);

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        await saveToken(data['token']);
        return data;
      } else {
        final errorData = jsonDecode(response.body);
        throw Exception(errorData['message'] ?? 'Error al iniciar sesión');
      }
    } on SocketException {
      throw Exception(
          'No se pudo conectar al servidor. Verifica que:\n'
          '1. El servidor esté corriendo\n'
          '2. Tu dispositivo esté en la misma red WiFi\n'
          '3. La IP del servidor sea correcta: ${ApiConfig.baseUrl}');
    } on HttpException {
      throw Exception('Error de conexión HTTP. Verifica la URL del servidor.');
    } on FormatException {
      throw Exception('Error al procesar la respuesta del servidor.');
    } catch (e) {
      if (e.toString().contains('timeout') || e.toString().contains('timed out')) {
        throw Exception(
            'Tiempo de espera agotado. Verifica que:\n'
            '1. El servidor esté corriendo en ${ApiConfig.baseUrl}\n'
            '2. Tu dispositivo esté en la misma red WiFi\n'
            '3. El firewall no esté bloqueando la conexión');
      }
      throw Exception('Error al iniciar sesión: ${e.toString()}');
    }
  }

  static Future<Map<String, dynamic>> register(
    String email,
    String password,
    String fullName,
    String? phone,
    String? location,
    DateTime dateOfBirth,
  ) async {
    try {
      final response = await http
          .post(
            Uri.parse('$baseUrl/auth/register'),
            headers: await getHeaders(includeAuth: false),
            body: jsonEncode({
              'email': email,
              'password': password,
              'fullName': fullName,
              'phone': phone,
              'location': location,
              'dateOfBirth': dateOfBirth.toIso8601String().split('T')[0],
            }),
          )
          .timeout(timeout);

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        await saveToken(data['token']);
        return data;
      } else {
        final errorData = jsonDecode(response.body);
        throw Exception(errorData['message'] ?? 'Error al registrar');
      }
    } on SocketException {
      throw Exception(
          'No se pudo conectar al servidor. Verifica que:\n'
          '1. El servidor esté corriendo\n'
          '2. Tu dispositivo esté en la misma red WiFi\n'
          '3. La IP del servidor sea correcta: ${ApiConfig.baseUrl}');
    } on HttpException {
      throw Exception('Error de conexión HTTP. Verifica la URL del servidor.');
    } on FormatException {
      throw Exception('Error al procesar la respuesta del servidor.');
    } catch (e) {
      if (e.toString().contains('timeout') || e.toString().contains('timed out')) {
        throw Exception(
            'Tiempo de espera agotado. Verifica que:\n'
            '1. El servidor esté corriendo en ${ApiConfig.baseUrl}\n'
            '2. Tu dispositivo esté en la misma red WiFi\n'
            '3. El firewall no esté bloqueando la conexión');
      }
      throw Exception('Error al registrar: ${e.toString()}');
    }
  }

  static Future<List<dynamic>> getAnimals() async {
    final response = await http.get(
      Uri.parse('$baseUrl/animals'),
      headers: await getHeaders(),
    );

    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    } else {
      throw Exception('Error al obtener animales');
    }
  }
}
