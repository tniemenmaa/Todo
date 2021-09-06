<template>
    <b-card no-body class="task" :data-id="task.id">
        <b-card-header class="row align-items-start m-0 p-0 pt-3" header-tag="header" role="tab">
            <!-- Display data --> 
            <b-card-text class="col-1"><b-icon class="drag-handle" icon="grip-vertical"></b-icon></b-card-text>
            <b-card-text class="col font-weight-bold text-left" v-b-toggle="'task-accordion-'+task.id"><span v-b-tooltip.hover title="Summary">{{ task.summary }}</span></b-card-text>
            <b-card-text class="col d-none d-lg-block"  v-b-tooltip.hover title="Created">{{ createdAt }}</b-card-text>
            <b-card-text class="col" v-b-tooltip.hover title="Deadline">{{ deadlineAt }}</b-card-text>
            <b-card-text class="col" v-b-tooltip.hover title="Priority">{{ task.priority }}</b-card-text>
            <b-card-text class="col" v-b-tooltip.hove title="Status">{{ taskState }}</b-card-text>
        </b-card-header>
        <b-collapse :id="'task-accordion-'+task.id" role="tabpanel" @show="onShow">
            <b-card-body>
                <task-editor :task="task" :autosave="true" @save="save"></task-editor>
                <!-- Subtasks --> 
                <p class="label text-left font-weight-bold mb-2">Subtasks</p>
                <div class="sub-task-container align-middle">
                    <!-- No tasks disclaimer --> 
                    <p class="no-items" v-if="!task.children || task.children.length === 0">No tasks</p>
                    <!-- Task loading spinner -->
                    <div class="text-center" v-if="loadingChildren">
                        <b-spinner label="Loading"></b-spinner>
                    </div>

                    <draggable class="sub-tasks" :list="task.children" group="tasks" handle=".drag-handle" ghost-class="ghost">                    
                        <task v-for="child in task.children" :key="child.id" v-bind:task="child" :parentId="task.id" @save="save" @remove="remove" />
                    </draggable>
                </div>
                <div class="mt-3 text-right">
                    <b-button pill variant="primary mr-1" v-b-modal="'new-task-' + task.id">New Subtask</b-button>
                    <b-button pill variant="danger" @click="remove(null)" type="danger">Delete</b-button>
                </div>
            </b-card-body>
        </b-collapse>
        <task-editor :id="'new-task-' + task.id" :autosave="false" :ismodal="true" @save="save"></task-editor>
    </b-card>
</template>
<script lang="js">
import Vue from 'vue';
import draggable from 'vuedraggable';
import taskEditor from './TaskEditor.vue';
import axios from 'axios';
import tasks from '../config/tasks';

export default Vue.extend({
    name: 'task',
    components: {
        draggable,
        taskEditor
    },
    data() {
        return {
            loadingChildren: false,
            config: {
                dateLocale: 'en-GB',
                
                validation: {
                    maxSummary: 128,
                    maxDescription: 1024
                },
                taskStates: tasks.states.options
            }
        }
    },
    props: {
        task: Object,
        parentId: { type: Number, default: null }
    },
    computed: {
        summaryState() {
            if ( !this.task.summary ) return null;
            if ( this.task.summary.length > this.config.validation.maxSummary ) return false;
            return true;
        },
        descriptionState() {
            if ( !this.task.description ) return null;
            if ( this.task.description.length > this.config.validation.maxDescription ) return false;
            return true;
        },
        deadlineState() {
            if ( !this.task.deadlineAt ) return null;
            if ( isNaN(Date.parse(this.task.deadlineAt)) ) return false;
            return true;
        },
        priorityState() {
            if ( typeof this.task.priority !== "number" ) return false;
            if ( !this.task.priority ) return null;
            if ( !Number.isInteger(this.task.priority) ) return false;
            if ( !Number.isSafeInteger(this.task.priority) ) return false;
            return true;
        },
        taskState() {
            return this.config.taskStates[this.task.status].text;
        },
        createdAt() {
            return new Date(this.task.createdAt).toLocaleString(this.config.dateLocale);
        },
        deadlineAt() {
            if ( this.task.deadlineAt ) {
                return new Date(this.task.deadlineAt).toLocaleDateString(this.config.dateLocale);
            }
            return null;
        }

    },
    created: function() {
        // this ensure state consistency with parent id
        if ( this.task.parentId !== this.parentId ) {
            this.task.parentId = this.parentId;    
            this.save(this.task);
        }
    },
    methods: {
        // Input change
        save(task, onSavedCallback) {
            if (!task.parentId && task.id != this.task.id) {
                task.parentId = this.task.id;
            }
            if (!onSavedCallback) {
                onSavedCallback = this.onSaved;
            }

            this.$emit("save", task, onSavedCallback);
        },
        onSaved(task) {
            if (!this.task.children) {
                this.task.children = [];
            }

            this.task.children.push(task);
        },
        remove(task, onRemovedCallback) {
            if (!onRemovedCallback && task) {
                onRemovedCallback = this.onRemoved;
            }
            if ( !task ) {
                task = this.task;
            }
            this.$emit("remove", task, onRemovedCallback );
        },
        onRemoved(data) {
           
            let i = this.task.children.findIndex(c => c.id == data.id);

            this.$delete(this.task.children, i);
        },
        onShow(){
            // Lazy load children when drilling down
            if ( !this.task.children ) {
                this.loadingChildren = true;
                axios.get(`/api/tasks/${this.task.id}`).then(r => {
                    this.task.children = r.data.children;
                }).finally(() => 
                {
                    this.loadingChildren = false;
                })   
            }
        }
    }
});
</script>