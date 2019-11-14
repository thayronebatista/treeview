import { Component, OnInit } from '@angular/core';
import { ViewService } from '../services/view.service';
import { iItem } from '../model/iItem';

@Component({
  selector: 'app-view',
  templateUrl: './view.component.html',
  styleUrls: ['./view.component.css'],
  providers: [ViewService]
})
export class ViewComponent implements OnInit {
  public items: iItem[] = [];
  public msgError: string = "";

  constructor(private api: ViewService) { }

  ngOnInit() {
    this.api.listarItens().then(res => {
      this.items = this.setList(res);
    }).catch(err => {
      this.msgError = `Não foi possível obter os dados da lista: ${err}`;
    });
  }

  public setList(itemList: any): iItem[] {
    let items: iItem[] = [];

    itemList.forEach(el => {
      items.push(el);
      if(el.parentId == null){
        el.name = "---------" + el.name;
      }
      if (el.children != null) {
        items = [...items, ...this.setList(el.children)]
        el.name = "----" + el.name;
      }
    });

    return items;
  }
}
