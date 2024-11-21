@extends('layouts.app')
@section('title','所持アイテム一覧')
@section('body')
    @section('userList','nav-link px-2 ')
    @section('userLeg','nav-link px-2')
    @section('player','nav-link px-2')
    @section('item','nav-link px-2')
    @section('personalItem','nav-link px-2  link-secondary')
    
    <h3>■所持アイテム一覧■</h3>
    <table class="table table-hover" id="table">
        <tr>
            <th>ID</th>
            <th>プレイヤー名</th>
            <th>アイテム名</th>
            <th>所持個数</th>
        </tr>
        @foreach($items as $item)
            <tr>
                <td>{{$item['id']}}</td>
                <td>{{$item['user_name']}}</td>
                <td>{{$item['item_name']}}</td>
                <td>{{$item['personal_item']}}</td>
            </tr>
        @endforeach
    </table>
@endsection
