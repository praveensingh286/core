import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() valueFromHome: any;
  @Output() cancelRegister = new EventEmitter();
model: any = {};
  constructor(private auth: AuthService) { }

  ngOnInit() {
  }
register = () => {
this.auth.register(this.model).subscribe(() => {
console.log('register successfully.');
}, error => {
  console.log('registration faild.');
});
}
cancel = () => {
  this.cancelRegister.emit(false);
  console.log('cancel');
}
}
