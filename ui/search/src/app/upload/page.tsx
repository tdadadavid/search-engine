'use client'

export default function Page(){
    return(
        <main className="">
            <form className="mx-auto my-32 p-4 max-w-[500px] bg-slate-800 rounded">
                <label htmlFor="doc" className="block text-center font-bold hover:cursor-pointer pb-4">Select Document for Upload</label>
                <input id="doc" type="file" name="doc" className="hover:cursor-pointer block m-auto bg-slate-600 px-3 py-1"/>

                <button type="submit" className="block my-2 mx-auto py-2 px-5 rounded-md bg-white hover:bg-slate-600 hover:text-white text-black">Upload</button>
            </form>
        </main>
    )
}