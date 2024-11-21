@extends('layouts.app')
@section('title','アカウント削除完了')
<body>
<div class="modal modal-sheet position-static d-block bg-body-secondary p-4 py-md-5" tabindex="-1" role="dialog"
     id="modalSignin">
    <div class="modal-dialog" role="document">
        <div class="modal-content rounded-4 shadow">
            <div class="modal-header p-5 pb-4 border-bottom-0">
                <form method="post" action="{{route('accounts.destroy')}}">
                    @csrf
                    <h1 class="fw-bold mb-0 fs-2">「{{$account['name']}}」を削除しました</h1>
                    <button type="submit">topへ</button>
                    <input type="hidden" name="id" value={{$account['id']}}>
                </form>
            </div>
        </div>
    </div>
</div>
