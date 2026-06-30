import { Component } from '@angular/core';
import { Task } from '../models/Task';
import { TaskItem } from '../task-item/task-item';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-task-list',
  imports: [TaskItem,FormsModule],
  templateUrl: './task-list.html',
  styleUrl: './task-list.scss',
})
export class TaskList {
  newTaskTitle:string='';
  newTaskPriority: 'low' | 'medium' | 'high' = 'low';
  tasks:Task[]=[
    {id:1,title:'title1',completed:true,priority:'high'},
    {id:2,title:'title2',completed:false,priority:'medium'},
    {id:3,title:'title3',completed:true,priority:'low'},
  ]
  onToggle(id:number){
  const task=this.tasks.find(t=>t.id===id);
  if(task){
    task.completed=!task.completed;
  }
}
addTask(){
  if(!this.newTaskTitle.trim()){
    return;
  }
  const allIds=this.tasks.map(t=>t.id);
  const highestId=Math.max(...allIds);
  const newId=highestId+1;
  const newTask:Task={
    id:newId,
    title:this.newTaskTitle,
    completed:false,
   priority:this.newTaskPriority
  };
  this.tasks.push(newTask);
  this.newTaskTitle='';
  this.newTaskPriority='low';
}
}


