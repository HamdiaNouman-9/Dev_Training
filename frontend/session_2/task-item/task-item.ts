import { Component,Input,Output,EventEmitter } from '@angular/core';
import { Task } from '../models/Task';

@Component({
  selector: 'app-task-item',
  imports: [],
  templateUrl: './task-item.html',
  styleUrl: './task-item.scss',
})
export class TaskItem {
  @Input() task!:Task;
  @Output() toggle=new EventEmitter<number>();
  onToggleClick(){
    this.toggle.emit(this.task.id);
  }
}
