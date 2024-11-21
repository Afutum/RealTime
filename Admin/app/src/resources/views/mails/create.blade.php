@extends('layouts.app')
@section('title','メール送信')
@section('body')
    <h5>メール作成</h5>
    <form method="post" action="{{route('createMail')}}">
        @csrf
        <div class="modal-body p-5 pt-0">
            <form class="">
                <div class="form-floating mb-3">
                    <input type="number" class="form-control rounded-3" id="floatingInput" name="id" min="0" required>
                    <label for="floatingInput">ユーザーID</label>
                </div>
                <div class="form-floating mb-3">
                    <input type="text" class="form-control rounded-3" id="floatingInput" name="text" required>
                    <label for="floatingInput">テキスト</label>
                </div>

                <select class="form-select" aria-label="Default select example" name="item_id">
                    <option value="0">アイテムなし</option>
                    @foreach($items as $item)
                        <option value={{$item['id']}}>{{$item['item_name']}}</option>
                    @endforeach
                </select>
                <br>
                <div class="form-floating mb-3">
                    <input type="number" class="form-control rounded-3" id="floatingInput" name="cnt" min="0" required>
                    <label for="floatingInput">個数</label>
                </div>
                <button class="w-100 mb-2 btn btn-lg rounded-3 btn-primary" type="submit">送信</button>
            </form>
            @if(!empty($success))
                <ul>
                    <li>送信完了</li>
                </ul>
            @endif
        </div>
    </form>
@endsection
