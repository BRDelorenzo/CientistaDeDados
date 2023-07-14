from django.shortcuts import render
from django.shortcuts import render
# Create your views here.



def home(request):
    return render(request,'pages/home.html',context={'name':'Giovanni Delorenzo'})