import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpEventType, HttpResponse, HttpRequest } from '@angular/common/http';
import { iItem } from "../model/iItem";

@Injectable({
  providedIn: 'root'
})

export class ViewService {
  private uri = "https://localhost:5001";

  constructor(private http: HttpClient) { }

  async listarItens(): Promise<iItem[]> {
    return new Promise((ok, erro) => {
      try {
        const req = new HttpRequest('GET', `${this.uri}/items`);

        this.http.request(req).subscribe((event) => {
          switch (event.type) {
            case HttpEventType.Response:
              if (event instanceof HttpResponse) {
                if (event.status == 200) {
                  ok(event.body);
                }
                else {
                  erro("Não foi possível obter os dados da API.");
                }
              }
              break;
            default:
              break;
          }
        });
      }
      catch (error) {
        erro(`Não foi possível obter os itens: ${error}`);
      }
    });
  }
}
